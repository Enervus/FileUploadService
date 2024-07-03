using FileUploadService.Domain.Entity;
using FileUploadService.Domain.Interfaces.Databases;
using FileUploadService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.DAL.Databases
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        public UnitOfWork(DbContext context, IBaseRepository<FileEntity> fileEntities)
        {
            _context = context;
            FileEntities = fileEntities;
        }
        public IBaseRepository<FileEntity> FileEntities { get; set ; }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
           return await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
