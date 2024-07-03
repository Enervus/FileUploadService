using Google.Protobuf.Collections;

namespace FileUploadService.Converters.DtosConverters
{
    public static class DtosConverter
    {
        public static Domain.Dto.UploadFileEntityDto UploadDtoConverter(UploadFileEntityDto dto)
        {
            if(dto == null)
            {
                throw new ArgumentNullException("Dto null exception (Converter)");
            }
            return new Domain.Dto.UploadFileEntityDto(dto.Name, dto.FileBytes.ToByteArray(), dto.ProjectId, dto.UserId);
        }

        public static Domain.Dto.FileEntityDto FileEntityDtoConverter(FileEntityDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Dto null exception (Converter)");
            }
            return new Domain.Dto.FileEntityDto(dto.Id, dto.Name, dto.ProjectId);
        }

        public static Domain.Dto.DownloadFileEntityDto DownloadDtoConverter(DownloadFileEntityDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Dto null exception (Converter)");
            }
            return new Domain.Dto.DownloadFileEntityDto(dto.Name, dto.ProjectId);
        }

        public static FileEntityDto FileEntityDtoConverter(Domain.Dto.FileEntityDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Dto null exception (Converter)");
            }
            return new FileEntityDto
            {
               Id = dto.Id, 
               Name = dto.Name,
               ProjectId = dto.ProjectId, 
            };
        }

        public static DownloadedFileEntityDto DownloadedDtoConverter(Domain.Dto.DownloadedFileEntityDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Dto null exception (Converter)");
            }
            return new DownloadedFileEntityDto
            {
               Id = dto.Id,
               Name = dto.Name, 
               FileURL = dto.FileURL,
            };
        }
    }
}
