using ECommerce.UseCases.Common;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetAllProductsQuery(int PageNumber = 1, int PageSize = 10)
    : IRequest<Result<PaginatedResult<GetAllProductsResponse>>>;

internal sealed class GetAllProductsHandler(IProductQueryService productQueryService)
    : IRequestHandler<GetAllProductsQuery, Result<PaginatedResult<GetAllProductsResponse>>>
{
    public async Task<Result<PaginatedResult<GetAllProductsResponse>>> Handle(
        GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var pagination = new PaginationParams
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var products = await productQueryService.GetProductsPaginatedAsync(pagination, cancellationToken);

        return Result.Success(products);
    }
}