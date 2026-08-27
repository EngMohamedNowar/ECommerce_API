using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Identity;

public sealed class RefreshToken : BaseEntity
{
    public string TokenHash { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = null!;
    public DateTimeOffset ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }

    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;

    private RefreshToken() { }

    private RefreshToken(string tokenHash, Guid userId, DateTimeOffset expiresAt)
    {
        TokenHash = tokenHash;
        UserId = userId;
        ExpiresAt = expiresAt;
    }

    public static Result<RefreshToken> Create(string tokenHash, Guid userId, DateTimeOffset expiresAt)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
            return Result.Failure<RefreshToken>(Error.Validation("RefreshToken.Hash", "الرمز غير صالح."));

        return Result.Success(new RefreshToken(tokenHash, userId, expiresAt));
    }

    public Result Revoke()
    {
        if (IsRevoked)
            return Result.Failure(Error.Conflict("RefreshToken.Revoked", "الرمز ملغي من قبل."));

        IsRevoked = true;
        RevokedAt = DateTimeOffset.UtcNow;
        MarkAsUpdated();

        return Result.Success();
    }
}