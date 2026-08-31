using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases;

public sealed class ProductFilterService(IRepository<Product> productRepository)
{
    public async Task<IReadOnlyList<Product>> GetFilteredProductsAsync(
        string? brand = null,
        string? type = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        CancellationToken ct = default)
    {
        var spec = new ProductsWithBrandAndTypeSpec(brand, type, minPrice, maxPrice);
        return await productRepository.GetAllWithSpecAsync(spec, ct);
    }
}