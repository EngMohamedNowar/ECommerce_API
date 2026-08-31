using ECommerce.Domain.Entities;
using ECommerce.Domain.Identity;
using ECommerce.Domain.OrderAggregate;
using ECommerce.UseCases.Auth.Dtos;
using ECommerce.UseCases.Orders.Dtos;
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

        config.NewConfig<AppUser, UserResponse>()
            .Map(dest => dest.Email, src => src.Email!);

        config.NewConfig<Order, OrderResponse>()
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.Total, src => src.GetTotal())
            .Map(dest => dest.DeliveryMethodId, src => src.DeliveryMethod!.Id)
            .Map(dest => dest.DeliveryMethodName, src => src.DeliveryMethod!.ShortName)
            .Map(dest => dest.DeliveryDuration, src => src.DeliveryMethod!.DeliveryTime)
            .Map(dest => dest.DeliveryPrice, src => src.DeliveryMethod!.Price);

        config.NewConfig<OrderItem, OrderItemResponse>()
            .Map(dest => dest.ProductId, src => src.ItemOrdered.ProductId)
            .Map(dest => dest.ProductName, src => src.ItemOrdered.ProductName)
            .Map(dest => dest.PictureUrl, src => src.ItemOrdered.PictureUrl)
            .Map(dest => dest.Total, src => src.Price * src.Quantity);

        config.NewConfig<DeliveryMethod, DeliveryMethodResponse>();
    }
}