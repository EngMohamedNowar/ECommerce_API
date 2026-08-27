namespace ECommerce.UseCases.Auth.Dtos;

public sealed record UserResponse(
    Guid Id,
    string Email,
    string DisplayName,
    IReadOnlyList<string> Roles);