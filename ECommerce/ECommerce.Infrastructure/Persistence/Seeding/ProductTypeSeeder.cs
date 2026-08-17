using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Data;
using ECommerce.Infrastructure.Persistence.Seeding.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Seeding
{
    public class ProductTypeSeeder(StoreDbContext dbContext) : IDataSeeder
    {
        public int Order => 2;

        public async Task SeedAsync(CancellationToken ct = default)
            => await JsonSeeder.SeedIfEmpty<ProductType, ProductTypeSeedData>
                (dbContext.ProductTypes, "ProductBrands.json", r => ProductType.Create(r.Id, r.Name));
    }
}
