using ECommerce.Domain.OrderAggregate;

namespace ECommerce.Tests;

internal static class OrderFactory
{
    public static Address ValidAddress() =>
        Address.Create("Mohamed", "Nowar", "Street 1", "Cairo", "Cairo", "12345").Value;

    public static DeliveryMethod ValidDelivery(decimal price = 10m) =>
        DeliveryMethod.Create("UPS", "Express", "1-3 Days", price).Value;

    public static OrderItem ValidItem(decimal price = 50m, int quantity = 2) =>
        OrderItem.Create(
            new ProductItemOrdered(Guid.NewGuid(), "Keyboard", "https://img/keyboard.png"),
            price,
            quantity).Value;

    public static Order ValidOrder(string email = "buyer@test.com")
    {
        var result = Order.Create(email, ValidAddress(), ValidDelivery(), [ValidItem()]);
        return result.Value;
    }
}
