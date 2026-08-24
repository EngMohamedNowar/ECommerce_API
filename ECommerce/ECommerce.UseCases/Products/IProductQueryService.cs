using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products;

public interface IProductQueryService
{
    Task<IReadOnlyList<GetAllProductsResponse>> GetAllProductsAsync(CancellationToken ct = default);
    Task<GetProductByIdResponse?> GetProductByIdAsync(Guid id, CancellationToken ct = default);
}
