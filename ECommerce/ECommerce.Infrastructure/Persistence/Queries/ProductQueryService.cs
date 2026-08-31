using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

using ECommerce.UseCases.Common;

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

    public async Task<PaginatedResult<GetAllProductsResponse>> GetProductsPaginatedAsync(
        PaginationParams pagination, CancellationToken ct = default)
    {
        var query = dbContext.Products.AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ProjectToType<GetAllProductsResponse>()
            .ToListAsync(ct);

        return new PaginatedResult<GetAllProductsResponse>
        {
            Items = items,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize)
        };
    }
}
