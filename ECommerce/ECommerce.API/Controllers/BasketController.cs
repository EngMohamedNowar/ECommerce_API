using ECommerce.Domain.Basket;
using ECommerce.UseCases.Basket.Commands;
using ECommerce.UseCases.Basket.Queries;

namespace ECommerce.API.Controllers;

public class BasketController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("{basketId}")]
    public async Task<ActionResult<ApiResponse<CustomerBasket>>> GetBasket(
        string basketId, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetBasketQuery(basketId), ct);
        return FromResult(result);
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<CustomerBasket>>> UpdateBasket(
        CustomerBasket basket, CancellationToken ct = default)
    {
        var result = await mediator.Send(new UpdateBasketCommand(basket), ct);
        return FromResult(result, "تم تحديث السلة بنجاح.");
    }

    [HttpDelete("{basketId}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteBasket(
        string basketId, CancellationToken ct = default)
    {
        var result = await mediator.Send(new DeleteBasketCommand(basketId), ct);
        return FromResult(result, "تم حذف السلة.");
    }
}