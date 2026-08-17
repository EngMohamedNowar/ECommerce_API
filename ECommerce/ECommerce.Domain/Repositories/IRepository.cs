using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
