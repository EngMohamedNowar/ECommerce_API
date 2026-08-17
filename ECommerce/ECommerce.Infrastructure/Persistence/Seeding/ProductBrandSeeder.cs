using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Data;
using ECommerce.Infrastructure.Persistence.Seeding.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Seeding
{
    public class ProductBrandSeeder(StoreDbContext dbContext): IDataSeeder
    {
        public int Order => 1;

        public async Task SeedAsync(CancellationToken ct = default)
            => await JsonSeeder.SeedIfEmpty<ProductBrand, ProductBrandSeedModel>
                (dbContext.ProductBrands, "ProductBrands.json", r => ProductBrand.Create(r.Id,r.Name));
    }
}
