using ECommerce.Domain.Entities;
using ECommerce.UseCases.Products.Dtos;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.UseCases
{
    public class MappingConfigure : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Product, GetProductByIdResponse>()
                .Map(dest => dest.ProductBrand, src => src.ProductBrand.Name)
                .Map(dest => dest.ProductType, src => src.ProductType.Name);

        }
    }
}
