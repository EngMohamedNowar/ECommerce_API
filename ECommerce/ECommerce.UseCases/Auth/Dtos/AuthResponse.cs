namespace ECommerce.UseCases.Auth.Dtos;

public sealed record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt);