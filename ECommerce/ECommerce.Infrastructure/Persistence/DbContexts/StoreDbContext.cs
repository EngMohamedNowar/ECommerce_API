using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.DbContexts;

public class StoreDbContext(
    DbContextOptions<StoreDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductBrand> ProductBrands => Set<ProductBrand>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
