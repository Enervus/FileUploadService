using FileUploadService.Application.Resources;
using FileUploadService.Domain.Entity;
using FileUploadService.Domain.Enum;
using FileUploadService.Domain.Interfaces.Validations;
using FileUploadService.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Application.Validations.FluentValidations.FileEntityValidations
{
    public class FileEntityValidator : IFileEntityValidator
    {
        public BaseResult FileUploadValidator(FileEntity file)
        {
            if(file != null)
            {
                return new BaseResult
                {
                    ErrorMessage = ErrorMessage.FileEntityAlreadyExists,
                    ErrorCode = (int)ErrorCodes.FileEntityAlreadyExists,
                };
            }
            return new BaseResult();
        }

        public BaseResult ValidateOnNull(FileEntity model)
        {
            if(model == null)
            {
                return new BaseResult
                {
                    ErrorMessage = ErrorMessage.FileEntityNotFound,
                    ErrorCode = (int)ErrorCodes.FileEntityNotFound,
                };
            }
            return new BaseResult();
        }
    }
}
