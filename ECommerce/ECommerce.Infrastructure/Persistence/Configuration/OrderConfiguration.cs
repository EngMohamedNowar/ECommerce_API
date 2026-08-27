using ECommerce.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.Property(o => o.BuyerEmail)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.WithOwner();
            address.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            address.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            address.Property(a => a.Street).IsRequired().HasMaxLength(200);
            address.Property(a => a.City).IsRequired().HasMaxLength(100);
            address.Property(a => a.State).IsRequired().HasMaxLength(100);
            address.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
        });

        builder.Property(o => o.Subtotal)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(o => o.PaymentIntentId)
            .HasMaxLength(200);

        builder.HasOne(o => o.DeliveryMethod)
            .WithMany(d => d.Orders)
            .HasForeignKey(o => o.DeliveryMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.BuyerEmail);
        builder.HasIndex(o => o.Status);
    }
}