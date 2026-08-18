using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Queries;

public class ProductQueryService(StoreDbContext dbContext) : IProductQueryService
{
    public async Task<IReadOnlyList<GelAllProductsResponse>> GelAllProductsAsync(CancellationToken ct = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .ProjectToType<GelAllProductsResponse>()
            .ToListAsync(ct);
    }

    public async Task<GetProductByIdResponse?> GetProductByIdResponse(Guid Id, CancellationToken ct = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == Id)
            .ProjectToType<GetProductByIdResponse>()
            .FirstOrDefaultAsync(ct);
    }
}
