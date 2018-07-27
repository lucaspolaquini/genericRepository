﻿using Imobiliaria.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliaria.Repository.Base
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>  where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filtro)
        {
            return _dbContext.Set<TEntity>().AsNoTracking().Where(filtro).ToList();
        }
    }
}
