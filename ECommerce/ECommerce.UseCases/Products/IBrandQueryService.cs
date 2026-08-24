using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products;

public interface IBrandQueryService
{
    Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken ct = default);
}
