using ECommerce.Domain.OrderAggregate;
using ECommerce.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public class DeliveryMethodSeeder(StoreDbContext dbContext) : IDataSeeder
{
    public int Order => 3;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        if (await dbContext.DeliveryMethods.AnyAsync(ct))
            return;

        var methods = new[]
        {
            DeliveryMethod.Create("DHL", "Fastest standard delivery", "3-5 Days", 120),
            DeliveryMethod.Create("UPS", "Express delivery", "1-3 Days", 250),
            DeliveryMethod.Create("FEDEX", "Standard delivery", "5-7 Days", 60),
            DeliveryMethod.Create("GO", "Local courier", "1 Day", 40)
        };

        foreach (var method in methods)
            dbContext.DeliveryMethods.Add(method.Value);
    }
}