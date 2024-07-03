using FileUploadService.Application.Services;
using FileUploadService.Converters.BaseResultsConverters;
using FileUploadService.Converters.DtosConverters;
using Grpc.Core;
using ILogger = Serilog.ILogger;

namespace FileUploadService.Controllers
{
    public class FileEntityController: FileUploadService.FileUploadServiceBase
    {
        private readonly FileEntityService _fileEntityService;
        private readonly ILogger _logger;
        private readonly HttpContext _httpContext;
        public FileEntityController(FileEntityService fileEntityService, ILogger logger, HttpContext httpContext)
        {
            _fileEntityService = fileEntityService;
            _logger = logger;
            _httpContext = httpContext;
        }

        public override async Task<BaseResultFileEntityDto> UploadFileAsync(UploadFileEntityDto request, ServerCallContext context)
        {
            var response = await _fileEntityService.UploadFileEntityAsync(DtosConverter.UploadDtoConverter(request), _httpContext.Request.ContentType);

            if (response.IsSuccess)
            {
                return ResultsConverter.FileEntityResultConverter(response);
            }
            else
            {
                _logger.Error(response.ErrorMessage);
                return new BaseResultFileEntityDto 
                {
                    ErrorMessage = response.ErrorMessage,
                    ErrorCode = response.ErrorCode,
                };
            }
        }

        public override async Task<BaseResultDownloadedFileEntityDto> DownloadFileAsync(DownloadFileEntityDto request, ServerCallContext context)
        {
            var response = await _fileEntityService.DownloadFileEntityAsync(DtosConverter.DownloadDtoConverter(request));

            if (response.IsSuccess)
            {
                return ResultsConverter.DownloadedResultConverter(response);
            }
            else
            {
                _logger.Error(response.ErrorMessage);
                return new BaseResultDownloadedFileEntityDto
                {
                    ErrorMessage = response.ErrorMessage,
                    ErrorCode = response.ErrorCode,
                };
            }
        }

        public override async Task<BaseResultFileEntityDto> DeleteFileAsync(FileEntityId request, ServerCallContext context)
        {
            var response = await _fileEntityService.DeleteFileEntityAsync(request.Id);

            if(response.IsSuccess)
            {
                return ResultsConverter.FileEntityResultConverter(response);
            }
            else
            {
                _logger.Error(response.ErrorMessage);
                return new BaseResultFileEntityDto
                {
                    ErrorMessage = response.ErrorMessage,
                    ErrorCode = response.ErrorCode,
                };
            }
        }

        public override async Task<CollectionResultFileEntityDto> GetFilesByProjectId(ProjectId request, ServerCallContext context)
        {
            var response = await _fileEntityService.GetFilesByProjectId(request.Id);

            if (response.IsSuccess)
            {
                return ResultsConverter.FileEntityResultConverter(response);
            }
            else
            {
                _logger.Error(response.ErrorMessage);
                return new CollectionResultFileEntityDto
                {
                    ErrorMessage = response.ErrorMessage,
                    ErrorCode = response.ErrorCode,
                };
            }
        }
    }
}
