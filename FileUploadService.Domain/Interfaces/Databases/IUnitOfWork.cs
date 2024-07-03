using FileUploadService.Domain.Entity;
using FileUploadService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Interfaces.Databases
{
    public interface IUnitOfWork:IStateSaveChanges
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        IBaseRepository<FileEntity> FileEntities { get; set; }
    }
}
