using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Queries;

public class TypeQueryService(StoreDbContext dbContext) : ITypeQueryService
{
    public async Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypesAsync(CancellationToken ct = default)
    {
        return await dbContext.ProductTypes
            .AsNoTracking()
            .ProjectToType<GetAllTypesResponse>()
            .ToListAsync(ct);
    }
}
