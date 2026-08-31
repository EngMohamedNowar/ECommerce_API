using ECommerce.Domain.Common;

namespace ECommerce.Domain.Basket;

public sealed record BasketItem(
    Guid ProductId,
    string ProductName,
    decimal Price,
    string PictureUrl,
    string ProductBrand,
    string ProductType,
    int Quantity)
{
    public static Result<BasketItem> Create(
        Guid productId,
        string productName,
        decimal price,
        string pictureUrl,
        string productBrand,
        string productType,
        int quantity)
    {
        if (productId == Guid.Empty)
            return Result.Failure<BasketItem>(Error.Validation("BasketItem.ProductId", "المنتج مطلوب."));

        if (string.IsNullOrWhiteSpace(productName))
            return Result.Failure<BasketItem>(Error.Validation("BasketItem.ProductName", "اسم المنتج مطلوب."));

        if (price <= 0)
            return Result.Failure<BasketItem>(Error.Validation("BasketItem.Price", "سعر المنتج غير صالح."));

        if (quantity <= 0)
            return Result.Failure<BasketItem>(Error.Validation("BasketItem.Quantity", "الكمية لازم تكون أكبر من صفر."));

        return Result.Success(new BasketItem(productId, productName, price, pictureUrl, productBrand, productType, quantity));
    }
}