﻿using FileUploadService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.DAL.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null) 
            {
                throw new ArgumentNullException("Entity is null");
            }
            await _dbContext.AddAsync(entity);
            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public void Remove(TEntity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("Entity is null");
            }
            _dbContext.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
