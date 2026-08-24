namespace ECommerce.Infrastructure.Persistence.Seeding.Data.Models;

public sealed class ProductSeedModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string PictureUrl { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid ProductBrandId { get; set; }
    public Guid ProductTypeId { get; set; }
}
