using ECommerce.Domain.Identity;
using ECommerce.UseCases.Auth.Contracts;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public class AppIdentitySeeder(
    RoleManager<IdentityRole<Guid>> roleManager,
    UserManager<AppUser> userManager) : IDataSeeder
{
    public int Order => 0;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        const string adminEmail = "admin@ecommerce.com";

        if (await userManager.FindByEmailAsync(adminEmail) is not null)
            return;

        var adminResult = AppUser.Create("Admin", adminEmail);
        if (adminResult.IsFailure)
            return;

        var result = await userManager.CreateAsync(adminResult.Value, "Admin@12345");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminResult.Value, Roles.Admin);
    }
}