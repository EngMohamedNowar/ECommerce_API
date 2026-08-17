using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.PictureUrl)
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // العلاقة مع Brand
            builder.HasOne(p => p.ProductBrand)
                .WithMany(x => x.Products)
                .HasForeignKey(p => p.ProductBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // العلاقة مع ProductType
            builder.HasOne(p => p.ProductType)
                .WithMany(x => x.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Price);
        }
    }
}