using ECommerce.Domain.OrderAggregate;

namespace ECommerce.UseCases.Orders.Dtos;

public sealed record AddressResponse(
    string FirstName,
    string LastName,
    string Street,
    string City,
    string State,
    string ZipCode);