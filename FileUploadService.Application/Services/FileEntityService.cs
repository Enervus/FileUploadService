using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using FileUploadService.Application.Resources;
using FileUploadService.Application.Validations.FluentValidations.FileEntityValidations;
using FileUploadService.Domain.Dto;
using FileUploadService.Domain.Entity;
using FileUploadService.Domain.Enum;
using FileUploadService.Domain.Interfaces.Databases;
using FileUploadService.Domain.Interfaces.Repositories;
using FileUploadService.Domain.Interfaces.Services;
using FileUploadService.Domain.Interfaces.Validations;
using FileUploadService.Domain.Result;
using FileUploadService.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Application.Services
{
    public class FileEntityService : IFileEntityService
    {
        private readonly IBaseRepository<FileEntity> _fileEntityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IFileEntityValidator _fileEntityValidator;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public FileEntityService(IBaseRepository<FileEntity> fileEntityRepository, IMapper mapper, ILogger logger, IFileEntityValidator fileEntityValidator, IAmazonS3 s3Client, IOptions<AwsSettings> awsOptions, IUnitOfWork unitOfWork)
        {
            _fileEntityRepository = fileEntityRepository;
            _mapper = mapper;
            _logger = logger;
            _fileEntityValidator = fileEntityValidator;
            _s3Client = s3Client;
            _bucketName = awsOptions.Value.BucketName;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResult<FileEntityDto>> UploadFileEntityAsync(UploadFileEntityDto dto, string contentType)
        {
            var fileEntity = await _fileEntityRepository.GetAll().FirstOrDefaultAsync(x=>x.Name == dto.Name);
            var result =  _fileEntityValidator.FileUploadValidator(fileEntity);

            if (!result.IsSuccess)
            {
                _logger.Warning(result.ErrorMessage);
                return new BaseResult<FileEntityDto>
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    fileEntity = new FileEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = dto.Name,
                        ProjectId = dto.ProjectId,
                        CreatedBy = dto.UserId
                    };

                    await _unitOfWork.FileEntities.CreateAsync(fileEntity);
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var encryptedBytes = Encoding.UTF8.GetString(dto.FileBytes);

                    using (var stream = new MemoryStream(dto.FileBytes))
                    {
                        
                        var putRequest = new PutObjectRequest
                        {
                            BucketName = _bucketName,
                            Key = $"{ fileEntity.Name}_{fileEntity?.ProjectId}_{fileEntity?.CreatedBy}",
                            InputStream = stream,
                            ContentType = contentType,
                        };
                        var response = await _s3Client.PutObjectAsync(putRequest);
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }
            return new BaseResult<FileEntityDto>
            {
                Data = _mapper.Map<FileEntityDto>(dto),
            };

        }
        public async Task<BaseResult<DownloadedFileEntityDto>> DownloadFileEntityAsync(DownloadFileEntityDto dto)
        {
            DownloadedFileEntityDto downloadedFileDto;
            var fileEntity = await _fileEntityRepository.GetAll().FirstOrDefaultAsync(x=>x.Name == dto.Name);
            var result = _fileEntityValidator.ValidateOnNull(fileEntity);
            if(!result.IsSuccess)
            {
                _logger.Warning(result.ErrorMessage);
                return new BaseResult<DownloadedFileEntityDto>
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }

            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = $"{fileEntity.Id}_{fileEntity.Name}_{fileEntity?.ProjectId}_{fileEntity?.CreatedBy}",
                Expires = DateTime.UtcNow.AddDays(1),
            };

            string url = _s3Client.GetPreSignedURL(request);
            downloadedFileDto = new DownloadedFileEntityDto(fileEntity.Id, fileEntity.Name, url);

            return new BaseResult<DownloadedFileEntityDto>
            {
                Data = downloadedFileDto,
            };
        }
        public async Task<BaseResult<FileEntityDto>> DeleteFileEntityAsync(string id)
        {
            var fileEntity = await _fileEntityRepository.GetAll().FirstOrDefaultAsync(x=>x.Id == id);
            var result = _fileEntityValidator.ValidateOnNull(fileEntity);

            if (!result.IsSuccess)
            {
                return new BaseResult<FileEntityDto>
                {
                    ErrorMessage = ErrorMessage.FileEntityNotFound,
                    ErrorCode = (int)ErrorCodes.FileEntityNotFound,
                };
            }
            using(var transaction =  await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    _unitOfWork.FileEntities.Remove(fileEntity);
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = $"{fileEntity.Name}_{fileEntity?.ProjectId}_{fileEntity?.CreatedBy}",
                    };
                    await _s3Client.DeleteObjectAsync(deleteRequest);
                }
                catch(Exception)
                {
                    await transaction.RollbackAsync();
                }

                return new BaseResult<FileEntityDto>
                {
                    Data = _mapper.Map<FileEntityDto>(fileEntity),
                };    
            }
        }

        public async Task<CollectionResult<FileEntityDto>> GetFilesByProjectId(string projectId)
        {
            FileEntityDto[] files;
            files = _fileEntityRepository.GetAll().Select(x=>new FileEntityDto(x.Id,x.Name,x.ProjectId)).Where(x=>x.ProjectId == projectId).ToArray();

            if (!files.Any())
            {
                _logger.Warning(ErrorMessage.FileEntitiesNotFound);
                return new CollectionResult<FileEntityDto>
                {
                    ErrorMessage = ErrorMessage.FileEntitiesNotFound,
                    ErrorCode = (int)ErrorCodes.FileEntitiesNotFound,
                };
            }

            return new CollectionResult<FileEntityDto>
            {
                Data = files,
                Count = files.Length,
            };
        }

       
    }
}
