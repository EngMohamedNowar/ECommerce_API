using ECommerce.UseCases.Orders.Commands;
using ECommerce.UseCases.Orders.Dtos;
using ECommerce.UseCases.Orders.Queries;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.API.Controllers;

[Authorize]
public class OrdersController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrderResponse>>>> GetMyOrders(
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetOrdersForUserQuery(), ct);
        return FromResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> GetOrderById(
        Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetOrderByIdQuery(id), ct);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> CreateOrder(
        CreateOrderRequest request, CancellationToken ct = default)
    {
        var result = await mediator.Send(new CreateOrderCommand(
            request.BasketId,
            request.DeliveryMethodId,
            request.ShippingAddress), ct);

        return FromResult(result, "تم إنشاء الطلب بنجاح.");
    }

    [HttpGet("delivery-methods")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DeliveryMethodResponse>>>> GetDeliveryMethods(
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetDeliveryMethodsQuery(), ct);
        return FromResult(result);
    }
}