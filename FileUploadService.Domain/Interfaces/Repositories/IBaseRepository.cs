using FileUploadService.Domain.Interfaces.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity>: IStateSaveChanges
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> CreateAsync(TEntity entity);
        void Remove(TEntity entity);
    }
}
