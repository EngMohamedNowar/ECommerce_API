using ECommerce.Domain.Identity;
using ECommerce.Domain.Specifications;

namespace ECommerce.UseCases.Auth.Specifications;

public sealed class RefreshTokenByHashSpec(string tokenHash)
    : BaseSpecification<RefreshToken>(t => t.TokenHash == tokenHash);

public sealed class ActiveRefreshTokensForUserSpec(Guid userId)
    : BaseSpecification<RefreshToken>(
        t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > DateTimeOffset.UtcNow);