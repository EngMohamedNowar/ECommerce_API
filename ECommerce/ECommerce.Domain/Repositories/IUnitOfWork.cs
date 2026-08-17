using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>()where T : BaseEntity;
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
