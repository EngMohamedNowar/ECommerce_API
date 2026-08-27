using ECommerce.Domain.OrderAggregate;

namespace ECommerce.UseCases.Orders.Dtos;

public sealed record CreateOrderRequest(
    string BasketId,
    Guid DeliveryMethodId,
    AddressRequest ShippingAddress);

public sealed record AddressRequest(
    string FirstName,
    string LastName,
    string Street,
    string City,
    string State,
    string ZipCode);