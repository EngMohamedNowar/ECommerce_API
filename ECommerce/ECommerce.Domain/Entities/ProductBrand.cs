using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class ProductBrand : BaseEntity
{
    public string Name { get; private set; } = null!;
    public ICollection<Product> Products { get; private set; } = [];

    private ProductBrand(string name)
    {
        Name = name;
    }

    public static ProductBrand Create(Guid Id,string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException("Brand name is required");

        return new ProductBrand(Name);
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Brand name is required");

        Name = newName;
        MarkAsUpdated();
    }
}