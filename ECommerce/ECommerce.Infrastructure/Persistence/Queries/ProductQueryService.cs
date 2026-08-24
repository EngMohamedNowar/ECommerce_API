using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Queries;

public class ProductQueryService(StoreDbContext dbContext) : IProductQueryService
{
    public async Task<IReadOnlyList<GetAllProductsResponse>> GetAllProductsAsync(CancellationToken ct = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .ProjectToType<GetAllProductsResponse>()
            .ToListAsync(ct);
    }

    public async Task<GetProductByIdResponse?> GetProductByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .ProjectToType<GetProductByIdResponse>()
            .FirstOrDefaultAsync(ct);
    }
}
