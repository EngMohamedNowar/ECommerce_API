using ECommerce.Domain.OrderAggregate;

namespace ECommerce.UseCases.Orders.Dtos;

public sealed record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal Price,
    int Quantity,
    decimal Total);