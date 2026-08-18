namespace ECommerce.UseCases.Products.Dtos;

public record GetProductByIdResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    string ProductType,
    string ProductBrand
);
