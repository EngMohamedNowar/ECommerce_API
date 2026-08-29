using ECommerce.UseCases.Orders.Dtos;
using ECommerce.UseCases.Payments.Commands;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.API.Controllers;

public class PaymentsController(IMediator mediator) : ApiControllerBase
{
    [Authorize]
    [HttpPost("{orderId:guid}")]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> CreateOrUpdatePaymentIntent(
        Guid orderId,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new CreateOrUpdatePaymentIntentCommand(orderId), ct);
        return FromResult(result, "تم إنشاء أو تحديث نية الدفع بنجاح.");
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook(CancellationToken ct = default)
    {
        using var reader = new StreamReader(HttpContext.Request.Body);
        var json = await reader.ReadToEndAsync(ct);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        var result = await mediator.Send(new ProcessStripeWebhookCommand(json, signature), ct);
        return result.IsFailure ? Problem(result) : Ok();
    }
}
