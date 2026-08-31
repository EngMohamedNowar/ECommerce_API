using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Identity;

public sealed class AppUser : IdentityUser<Guid>
{
    public string DisplayName { get; private set; } = null!;

    public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];

    private AppUser() { }

    private AppUser(string displayName, string email)
    {
        DisplayName = displayName;
        UserName = email;
        Email = email;
        EmailConfirmed = true;
        SecurityStamp = Guid.NewGuid().ToString();
    }

    public static Result<AppUser> Create(string displayName, string email)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            return Result.Failure<AppUser>(Error.Validation("Auth.DisplayName", "الاسم مطلوب."));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return Result.Failure<AppUser>(Error.Validation("Auth.Email", "بريد إلكتروني غير صالح."));

        return Result.Success(new AppUser(displayName, email));
    }

    public void UpdateProfile(string displayName)
    {
        DisplayName = displayName;
    }
}