using ECommerce.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configuration;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.ToTable("DeliveryMethods");

        builder.Property(d => d.ShortName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(d => d.DeliveryTime)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}