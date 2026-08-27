using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.OrderAggregate;

public sealed class OrderItem : BaseEntity
{
    public ProductItemOrdered ItemOrdered { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    private OrderItem() { }

    private OrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity)
    {
        ItemOrdered = itemOrdered;
        Price = price;
        Quantity = quantity;
    }

    public static Result<OrderItem> Create(ProductItemOrdered itemOrdered, decimal price, int quantity)
    {
        if (itemOrdered is null)
            return Result.Failure<OrderItem>(Error.Validation("OrderItem.Product", "المنتج مطلوب."));

        if (price <= 0)
            return Result.Failure<OrderItem>(Error.Validation("OrderItem.Price", "سعر المنتج غير صالح."));

        if (quantity <= 0)
            return Result.Failure<OrderItem>(Error.Validation("OrderItem.Quantity", "الكمية لازم تكون أكبر من صفر."));

        return Result.Success(new OrderItem(itemOrdered, price, quantity));
    }
}