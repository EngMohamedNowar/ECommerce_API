using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetAllProductsQuery() : IRequest<Result<IReadOnlyList<GetAllProductsResponse>>>;

internal sealed class GetAllProductsHandler(IProductQueryService productQueryService)
    : IRequestHandler<GetAllProductsQuery, Result<IReadOnlyList<GetAllProductsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllProductsResponse>>> Handle(
        GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productQueryService.GetAllProductsAsync(cancellationToken);
        return Result.Success<IReadOnlyList<GetAllProductsResponse>>(products);
    }
}
