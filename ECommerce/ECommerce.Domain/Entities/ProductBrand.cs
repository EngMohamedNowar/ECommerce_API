using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class ProductBrand : BaseEntity
{
    public string Name { get; private set; } = null!;
    public ICollection<Product> Products { get; private set; } = [];

    private ProductBrand(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static ProductBrand Create(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Brand name is required");

        return new ProductBrand(id, name);
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Brand name is required");

        Name = newName;
        MarkAsUpdated();
    }
}
