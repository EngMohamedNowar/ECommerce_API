using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Queries;

public class BrandQueryService(StoreDbContext dbContext) : IBrandQueryService
{
    public async Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken ct = default)
    {
        return await dbContext.ProductBrands
            .AsNoTracking()
            .ProjectToType<GetAllBrandsResponse>()
            .ToListAsync(ct);
    }
}
