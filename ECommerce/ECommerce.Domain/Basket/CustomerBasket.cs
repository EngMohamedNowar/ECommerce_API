namespace ECommerce.Domain.Basket;

public sealed class CustomerBasket
{
    public string Id { get; set; } = null!;
    public List<BasketItem> Items { get; set; } = [];
    public Guid? DeliveryMethodId { get; set; }
    public decimal ShippingPrice { get; set; }

    public decimal Subtotal => Items.Sum(i => i.Price * i.Quantity);
    public decimal GetTotal() => Subtotal + ShippingPrice;

    public void SetDeliveryMethod(Guid deliveryMethodId, decimal shippingPrice)
    {
        DeliveryMethodId = deliveryMethodId;
        ShippingPrice = shippingPrice;
    }
}