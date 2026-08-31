using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ECommerce.UseCases.Auth.Contracts;

namespace ECommerce.API.Auth;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? Claims => httpContextAccessor.HttpContext?.User;

    public Guid? Id =>
        Guid.TryParse(Claims?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out var id) ? id : null;

    public string? Email => Claims?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

    public string? DisplayName => Claims?.FindFirst(JwtRegisteredClaimNames.Name)?.Value;

    public IEnumerable<string> Roles => Claims?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? [];

    public bool IsAuthenticated => Claims?.Identity?.IsAuthenticated ?? false;
}