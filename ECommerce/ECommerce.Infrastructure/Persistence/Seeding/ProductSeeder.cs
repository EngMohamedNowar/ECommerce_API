using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Data;
using ECommerce.Infrastructure.Persistence.Seeding.Data.Models;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public class ProductSeeder(StoreDbContext dbContext) : IDataSeeder
{
    public int Order => 3;

    public async Task SeedAsync(CancellationToken ct = default)
        => await JsonSeeder.SeedIfEmpty<Product, ProductSeedModel>(
            dbContext.Products,
            "Products.json",
            r =>
            {
                var brand = dbContext.ProductBrands.Find(r.ProductBrandId)
                            ?? throw new InvalidOperationException($"Product brand '{r.ProductBrandId}' not found for product '{r.Name}'.");
                var type = dbContext.ProductTypes.Find(r.ProductTypeId)
                           ?? throw new InvalidOperationException($"Product type '{r.ProductTypeId}' not found for product '{r.Name}'.");

                return Product.Create(
                        r.Id,
                        r.Name,
                        r.Description,
                        r.PictureUrl,
                        r.Price,
                        brand,
                        type)
                    .Value;
            });
}
