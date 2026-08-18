using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string PictureUrl { get; private set; } = null!;
    public decimal Price { get; private set; }

    public Guid ProductBrandId { get; private set; }
    public ProductBrand ProductBrand { get; private set; } = null!;

    public Guid ProductTypeId { get; private set; }
    public ProductType ProductType { get; private set; } = null!;

    private Product() { }

    private Product(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        ProductBrand productBrand,
        ProductType productType)
    {
        Name = name;
        Description = description;
        PictureUrl = pictureUrl;
        Price = price;
        ProductBrand = productBrand;
        ProductBrandId = productBrand.Id;
        ProductType = productType;
        ProductTypeId = productType.Id;
    }

    public static Result<Product> Create(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        ProductBrand productBrand,
        ProductType productType)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Product>(Error.Validation("Product.Name", "اسم المنتج مطلوب."));

        if (price <= 0)
            return Result.Failure<Product>(Error.Validation("Product.Price", "سعر المنتج لازم يكون أكبر من صفر."));

        if (productBrand is null)
            return Result.Failure<Product>(Error.Validation("Product.Brand", "ماركة المنتج مطلوبة."));

        if (productType is null)
            return Result.Failure<Product>(Error.Validation("Product.Type", "نوع المنتج مطلوب."));

        var product = new Product(
            name,
            description ?? string.Empty,
            pictureUrl ?? string.Empty,
            price,
            productBrand,
            productType);

        return Result.Success(product);
    }

    public Result UpdateDetails(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(Error.Validation("Product.Name", "اسم المنتج مطلوب."));

        if (price <= 0)
            return Result.Failure(Error.Validation("Product.Price", "سعر المنتج لازم يكون أكبر من صفر."));

        Name = name;
        Description = description ?? string.Empty;
        Price = price;
        MarkAsUpdated();

        return Result.Success();
    }

    public Result ChangePicture(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl))
            return Result.Failure(Error.Validation("Product.PictureUrl", "رابط الصورة مطلوب."));

        PictureUrl = pictureUrl;
        MarkAsUpdated();

        return Result.Success();
    }

    public Result ChangeBrand(ProductBrand productBrand)
    {
        if (productBrand is null)
            return Result.Failure(Error.Validation("Product.Brand", "ماركة المنتج مطلوبة."));

        ProductBrand = productBrand;
        ProductBrandId = productBrand.Id;
        MarkAsUpdated();

        return Result.Success();
    }

    public Result ChangeType(ProductType productType)
    {
        if (productType is null)
            return Result.Failure(Error.Validation("Product.Type", "نوع المنتج مطلوب."));

        ProductType = productType;
        ProductTypeId = productType.Id;
        MarkAsUpdated();

        return Result.Success();
    }
}