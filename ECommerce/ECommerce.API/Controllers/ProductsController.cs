using ECommerce.UseCases.Common;

namespace ECommerce.API.Controllers;

public class ProductsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductsResponse>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllProductsQuery(pageNumber, pageSize), ct);

        if (result.IsFailure)
            return Problem(result);

        var pagination = new PaginationMeta(
            result.Value.PageNumber,
            result.Value.PageSize,
            result.Value.TotalCount);

        return Success(result.Value.Items, "تم جلب المنتجات بنجاح", pagination);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<GetProductByIdResponse>>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), ct);
        return FromResult(result);
    }
}