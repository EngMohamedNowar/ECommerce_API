using ECommerce.Domain.Identity;

namespace ECommerce.UseCases.Auth.Contracts;

public interface IAuthTokenService
{
    string GenerateAccessToken(AppUser user, IEnumerable<string> roles);
    string GenerateRefreshToken();
    string HashRefreshToken(string refreshToken);
}