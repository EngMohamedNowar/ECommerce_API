using ECommerce.Domain.OrderAggregate;

namespace ECommerce.UseCases.Orders.Dtos;

public sealed record DeliveryMethodResponse(
    Guid Id,
    string ShortName,
    string Description,
    string DeliveryTime,
    decimal Price);