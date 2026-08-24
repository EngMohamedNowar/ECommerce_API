using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class ProductsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetAllProductsResponse>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllProductsQuery(), ct);
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetProductByIdResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), ct);
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}
