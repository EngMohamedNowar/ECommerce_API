using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Data;
using ECommerce.Infrastructure.Persistence.Seeding.Data.Models;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public class ProductTypeSeeder(StoreDbContext dbContext) : IDataSeeder
{
    public int Order => 2;

    public async Task SeedAsync(CancellationToken ct = default)
        => await JsonSeeder.SeedIfEmpty<ProductType, ProductTypeSeedData>
            (dbContext.ProductTypes, "ProductTypes.json", r => ProductType.Create(r.Id, r.Name));
}
