using ECommerce.UseCases.Common;

namespace ECommerce.UseCases.Products;

public interface IProductQueryService
{
    Task<IReadOnlyList<GetAllProductsResponse>> GetAllProductsAsync(CancellationToken ct = default);
    Task<GetProductByIdResponse?> GetProductByIdAsync(Guid id, CancellationToken ct = default);
    Task<PaginatedResult<GetAllProductsResponse>> GetProductsPaginatedAsync(PaginationParams pagination, CancellationToken ct = default);
}