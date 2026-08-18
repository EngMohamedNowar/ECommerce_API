using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products.Queries;

public sealed class GetProductByIdQuery(IProductQueryService productQueryService)
{
    public async Task<Result<GetProductByIdResponse>> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await productQueryService.GetProductByIdResponse(id, ct);

        if (product is null)
            return Result.Failure<GetProductByIdResponse>(
                Error.NotFound("Product.NotFound", "المنتج غير موجود."));

        return Result.Success(product);
    }
}
