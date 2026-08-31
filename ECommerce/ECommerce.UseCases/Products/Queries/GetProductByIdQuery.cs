using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Result<GetProductByIdResponse>>;

internal sealed class GetProductByIdHandler(IProductQueryService productQueryService)
    : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdResponse>>
{
    public async Task<Result<GetProductByIdResponse>> Handle(
        GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productQueryService.GetProductByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return Result.Failure<GetProductByIdResponse>(
                Error.NotFound("Product.NotFound", "المنتج غير موجود."));

        return Result.Success(product);
    }
}
