using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.OrderAggregate;

public sealed class DeliveryMethod : BaseEntity
{
    public string ShortName { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string DeliveryTime { get; private set; } = null!;
    public decimal Price { get; private set; }

    public ICollection<Order> Orders { get; private set; } = [];

    private DeliveryMethod() { }

    private DeliveryMethod(string shortName, string description, string deliveryTime, decimal price)
    {
        ShortName = shortName;
        Description = description;
        DeliveryTime = deliveryTime;
        Price = price;
    }

    public static Result<DeliveryMethod> Create(
        string shortName, string description, string deliveryTime, decimal price)
    {
        if (string.IsNullOrWhiteSpace(shortName))
            return Result.Failure<DeliveryMethod>(Error.Validation("DeliveryMethod.ShortName", "اسم طريقة التوصيل مطلوب."));

        if (price < 0)
            return Result.Failure<DeliveryMethod>(Error.Validation("DeliveryMethod.Price", "سعر التوصيل غير صالح."));

        return Result.Success(new DeliveryMethod(shortName, description ?? string.Empty, deliveryTime ?? string.Empty, price));
    }
}