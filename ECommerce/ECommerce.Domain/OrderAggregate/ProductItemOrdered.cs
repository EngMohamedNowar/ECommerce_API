namespace ECommerce.Domain.OrderAggregate;

public sealed record ProductItemOrdered(
    Guid ProductId,
    string ProductName,
    string PictureUrl);