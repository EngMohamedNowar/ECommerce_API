using ECommerce.Domain.OrderAggregate;

namespace ECommerce.UseCases.Orders.Dtos;

public sealed record OrderResponse(
    Guid Id,
    DateTimeOffset OrderDate,
    string BuyerEmail,
    string Status,
    decimal Subtotal,
    decimal Total,
    Guid DeliveryMethodId,
    string DeliveryMethodName,
    string DeliveryDuration,
    decimal DeliveryPrice,
    AddressResponse ShippingAddress,
    string? PaymentIntentId,
    IReadOnlyList<OrderItemResponse> Items);