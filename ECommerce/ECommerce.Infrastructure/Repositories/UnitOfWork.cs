using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using System.Collections.Concurrent;

namespace ECommerce.Infrastructure.Repositories;

public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
{
    private readonly ConcurrentDictionary<Type, object> _repos = new();
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        if (_repos.TryGetValue(type, out var repo))
            return (IRepository<T>)repo;
        var newRepo = new Repository<T>(dbContext);
        _repos.TryAdd(type, newRepo);
        return newRepo;

    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return dbContext.SaveChangesAsync(ct);
    }
}
