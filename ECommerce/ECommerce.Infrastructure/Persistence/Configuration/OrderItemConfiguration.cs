using ECommerce.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configuration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.OwnsOne(i => i.ItemOrdered, item =>
        {
            item.WithOwner();
            item.Property(io => io.ProductId).IsRequired();
            item.Property(io => io.ProductName).IsRequired().HasMaxLength(200);
            item.Property(io => io.PictureUrl).HasMaxLength(500);
        });

        builder.Property(i => i.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}