using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class ProductType : BaseEntity
{
    public string Name { get; private set; } = null!;
    public ICollection<Product> Products { get; private set; } = [];

    private ProductType(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static ProductType Create(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product type name is required");

        return new ProductType(id, name);
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Product type name is required");

        Name = newName;
        MarkAsUpdated();
    }
}
