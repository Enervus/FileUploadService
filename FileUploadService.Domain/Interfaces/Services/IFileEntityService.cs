using FileUploadService.Domain.Dto;
using FileUploadService.Domain.Entity;
using FileUploadService.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Interfaces.Services
{
    public interface IFileEntityService
    {
        Task<BaseResult<FileEntityDto>> UploadFileEntityAsync(UploadFileEntityDto dto, string contentType);
        Task<BaseResult<FileEntityDto>> DeleteFileEntityAsync(string id);
        Task<BaseResult<DownloadedFileEntityDto>> DownloadFileEntityAsync(DownloadFileEntityDto dto);
        Task<CollectionResult<FileEntityDto>> GetFilesByProjectId(string projectId);
    }
}
