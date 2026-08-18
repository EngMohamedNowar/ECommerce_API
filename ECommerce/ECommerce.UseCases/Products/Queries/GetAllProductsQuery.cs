using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.UseCases.Products.Queries;

public sealed class GetAllProductsQuery(IProductQueryService productQueryService)
{
    public async Task<Result<IReadOnlyList<GelAllProductsResponse>>> ExcuteAsync()
    {
        var products = await productQueryService.GelAllProductsAsync();
        return Result<IReadOnlyList<GetAllProductsQuery>>.Success(products);

    }
}
