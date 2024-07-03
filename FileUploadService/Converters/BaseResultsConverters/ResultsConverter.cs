using FileUploadService.Converters.DtosConverters;
using FileUploadService.Domain.Entity;
using FileUploadService.Domain.Result;

namespace FileUploadService.Converters.BaseResultsConverters
{
    public static class ResultsConverter
    {
        public static BaseResultFileEntityDto FileEntityResultConverter(Domain.Result.BaseResult<Domain.Dto.FileEntityDto> baseResult)
        {
            if (baseResult == null)
            {
                throw new ArgumentNullException("Result null exception (Converter)");
            }
            return new BaseResultFileEntityDto
            {
                IsSuccess = baseResult.IsSuccess,
                ErrorMessage = baseResult.ErrorMessage,
                ErrorCode = baseResult.ErrorCode,
                Data = DtosConverter.FileEntityDtoConverter(baseResult.Data),
            };
        }

        public static BaseResultDownloadedFileEntityDto DownloadedResultConverter(Domain.Result.BaseResult<Domain.Dto.DownloadedFileEntityDto> baseResult) 
        {
            if (baseResult == null)
            {
                throw new ArgumentNullException("Result null exception (Converter)");
            }
            return new BaseResultDownloadedFileEntityDto
            {
                IsSuccess = baseResult.IsSuccess,
                ErrorMessage = baseResult.ErrorMessage,
                ErrorCode = baseResult.ErrorCode,
                Data = DtosConverter.DownloadedDtoConverter(baseResult.Data),
            };
        }

        public static CollectionResultFileEntityDto FileEntityResultConverter(Domain.Result.CollectionResult<Domain.Dto.FileEntityDto> collectionResult)
        {
            if (collectionResult == null) 
            {
                throw new ArgumentNullException("Result null exception (Converter)");
            }

            var result = new CollectionResultFileEntityDto
            {
                IsSuccess = collectionResult.IsSuccess,
                ErrorMessage = collectionResult.ErrorMessage,
                ErrorCode = collectionResult.ErrorCode,
            };
            foreach (var dto in collectionResult.Data)
            {
                result.Data.Add(DtosConverter.FileEntityDtoConverter(dto));
            }
            return result;
        }
    }
}
