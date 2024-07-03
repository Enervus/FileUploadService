using FileUploadService.Domain.Entity;
using FileUploadService.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Interfaces.Validations
{
    public interface IFileEntityValidator: IBaseValidator<FileEntity>
    {
        BaseResult FileUploadValidator(FileEntity file);
    }
}
