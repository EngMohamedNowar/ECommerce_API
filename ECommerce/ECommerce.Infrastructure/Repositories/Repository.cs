using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repositories
{
    public sealed class Repository<T>(StoreDbContext dbContext)
        : IRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet.FindAsync([id], cancellationToken: ct);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            entity.MarkAsDeleted();
        }

    }
}