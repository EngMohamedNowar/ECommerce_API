using ECommerce.Domain.Entities;
using ECommerce.Domain.Specifications;

namespace ECommerce.UseCases.Specifications;

public sealed class ProductsWithBrandAndTypeSpec : BaseSpecification<Product>
{
    public ProductsWithBrandAndTypeSpec(string? brand, string? type, decimal? minPrice, decimal? maxPrice)
        : base(p =>
            (string.IsNullOrEmpty(brand) || p.ProductBrand.Name == brand) &&
            (string.IsNullOrEmpty(type) || p.ProductType.Name == type) &&
            (minPrice == null || p.Price >= minPrice) &&
            (maxPrice == null || p.Price <= maxPrice) &&
            !p.IsDeleted)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
        ApplyOrderBy(p => p.Price);
    }

    public ProductsWithBrandAndTypeSpec(string brand, string type)
        : this(brand, type, null, null)
    {
    }
}