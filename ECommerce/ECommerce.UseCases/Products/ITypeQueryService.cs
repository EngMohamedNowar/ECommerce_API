using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products;

public interface ITypeQueryService
{
    Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypesAsync(CancellationToken ct = default);
}
