using FileUploadService.Domain.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Application.Validations.FluentValidations.FileEntityValidations
{
    public class UploadFileEntityValidator: AbstractValidator<UploadFileEntityDto>
    {
        public UploadFileEntityValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.FileBytes).NotEmpty();
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
