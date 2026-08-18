using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products;

public interface IProductQueryService
{
    Task<IReadOnlyList<GelAllProductsResponse>> GelAllProductsAsync(CancellationToken ct = default);
    Task<GetProductByIdResponse?> GetProductByIdResponse(Guid Id,CancellationToken ct = default);

}
