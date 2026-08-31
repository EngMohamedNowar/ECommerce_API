using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.OrderAggregate;

public sealed class Order : BaseEntity
{
    public string BuyerEmail { get; private set; } = null!;
    public DateTimeOffset OrderDate { get; private set; }
    public Address ShippingAddress { get; private set; } = null!;

    public Guid? DeliveryMethodId { get; private set; }
    public DeliveryMethod? DeliveryMethod { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;
    private List<OrderItem> _items = [];

    public decimal Subtotal { get; private set; }
    public decimal GetTotal() => Subtotal + (DeliveryMethod?.Price ?? 0);

    public OrderStatus Status { get; private set; }
    public string? PaymentIntentId { get; private set; }

    private Order() { }

    private Order(
        string buyerEmail,
        Address shippingAddress,
        DeliveryMethod? deliveryMethod,
        List<OrderItem> items,
        decimal subtotal)
    {
        BuyerEmail = buyerEmail;
        OrderDate = DateTimeOffset.UtcNow;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        DeliveryMethodId = deliveryMethod?.Id;
        _items = items;
        Subtotal = subtotal;
        Status = OrderStatus.Pending;
    }

    public static Result<Order> Create(
        string buyerEmail,
        Address shippingAddress,
        DeliveryMethod? deliveryMethod,
        List<OrderItem> items)
    {
        if (string.IsNullOrWhiteSpace(buyerEmail))
            return Result.Failure<Order>(Error.Validation("Order.BuyerEmail", "بريد المشتري مطلوب."));

        if (shippingAddress is null)
            return Result.Failure<Order>(Error.Validation("Order.Address", "عنوان الشحن مطلوب."));

        if (deliveryMethod is null)
            return Result.Failure<Order>(Error.Validation("Order.DeliveryMethod", "طريقة التوصيل مطلوبة."));

        if (items.Count == 0)
            return Result.Failure<Order>(Error.Validation("Order.Items", "الأوردر لازم يحتوي على منتجات."));

        var subtotal = items.Sum(i => i.Price * i.Quantity);

        return Result.Success(new Order(buyerEmail, shippingAddress, deliveryMethod, items, subtotal));
    }

    public Result SetPaymentIntentId(string paymentIntentId)
    {
        if (string.IsNullOrWhiteSpace(paymentIntentId))
            return Result.Failure(Error.Validation("Order.PaymentIntent", "معرف الدفع مطلوب."));

        PaymentIntentId = paymentIntentId;
        MarkAsUpdated();

        return Result.Success();
    }

    public Result MarkAsPaid()
    {
        if (PaymentIntentId is null)
            return Result.Failure(Error.Conflict("Order.NotPaid", "الأوردر لم يُدفع بعد."));

        Status = OrderStatus.PaymentReceived;
        MarkAsUpdated();

        return Result.Success();
    }

    public Result MarkAsPaymentFailed()
    {
        Status = OrderStatus.PaymentFailed;
        MarkAsUpdated();

        return Result.Success();
    }

    public Result Cancel()
    {
        Status = OrderStatus.Cancelled;
        MarkAsUpdated();

        return Result.Success();
    }
}