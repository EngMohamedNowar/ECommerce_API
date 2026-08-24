using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class TypesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetAllTypesResponse>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllTypesQuery(), ct);
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}
