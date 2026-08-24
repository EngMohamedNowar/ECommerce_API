using ECommerce.Domain.Entities;
using ECommerce.UseCases.Products.Dtos;
using Mapster;

namespace ECommerce.UseCases;

public class MappingConfigure : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, GetProductByIdResponse>()
            .Map(dest => dest.ProductBrand, src => src.ProductBrand.Name)
            .Map(dest => dest.ProductType, src => src.ProductType.Name);

        config.NewConfig<Product, GetAllProductsResponse>()
            .Map(dest => dest.ProductBrand, src => src.ProductBrand.Name)
            .Map(dest => dest.ProductType, src => src.ProductType.Name);
    }
}
