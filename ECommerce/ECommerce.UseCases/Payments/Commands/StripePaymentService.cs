using ECommerce.Domain.Common;
using ECommerce.Domain.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Orders.Dtos;
using ECommerce.UseCases.Orders.Specifications;
using ECommerce.UseCases.Payments.Contracts;
using ECommerce.UseCases.Payments.Specifications;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Payments.Commands;

public sealed class StripePaymentService(
    IRepository<Order> orderRepo,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IStripePaymentIntentGateway stripeGateway,
    IOptions<StripeOptions> options) : IPaymentService
{
    private readonly StripeOptions _options = options.Value;

    public async Task<Result<OrderResponse>> CreateOrUpdatePaymentIntentAsync(
        Guid orderId,
        string buyerEmail,
        CancellationToken ct = default)
    {
        var order = await orderRepo.GetEntityWithSpecAsync(
            new OrderWithItemsByUserSpec(orderId, buyerEmail).AddDetails(),
            ct);

        if (order is null)
            return Result.Failure<OrderResponse>(
                Error.NotFound("Order.NotFound", "الطلب غير موجود."));

        var amountInMinorUnits = ToMinorUnits(order.GetTotal());

        Result<string> intentResult;
        if (string.IsNullOrWhiteSpace(order.PaymentIntentId))
        {
            intentResult = await stripeGateway.CreateAsync(
                amountInMinorUnits,
                _options.Currency,
                order.Id,
                ct);
        }
        else
        {
            intentResult = await stripeGateway.UpdateAmountAsync(
                order.PaymentIntentId,
                amountInMinorUnits,
                ct);
        }

        if (intentResult.IsFailure)
            return Result.Failure<OrderResponse>(intentResult.Error);

        var setResult = order.SetPaymentIntentId(intentResult.Value);
        if (setResult.IsFailure)
            return Result.Failure<OrderResponse>(setResult.Error);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success(mapper.Map<OrderResponse>(order));
    }

    public async Task<Result> ProcessWebhookAsync(
        string json,
        string signature,
        CancellationToken ct = default)
    {
        var parsed = stripeGateway.ParseWebhook(json, signature);
        if (parsed.IsFailure)
            return Result.Failure(parsed.Error);

        var stripeEvent = parsed.Value;
        if (stripeEvent.Type is not (
            StripeWebhookEventTypes.PaymentIntentSucceeded or
            StripeWebhookEventTypes.PaymentIntentPaymentFailed))
        {
            return Result.Success();
        }

        var order = await orderRepo.GetEntityWithSpecAsync(
            new OrderByPaymentIntentIdSpec(stripeEvent.PaymentIntentId),
            ct);

        if (order is null)
            return Result.Failure(
                Error.NotFound("Order.NotFound", "الطلب غير موجود لمعرف الدفع."));

        var statusResult = stripeEvent.Type == StripeWebhookEventTypes.PaymentIntentSucceeded
            ? order.MarkAsPaid()
            : order.MarkAsPaymentFailed();

        if (statusResult.IsFailure)
            return statusResult;

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    internal static long ToMinorUnits(decimal amount) =>
        (long)Math.Round(amount * 100m, MidpointRounding.AwayFromZero);
}

public static class StripeWebhookEventTypes
{
    public const string PaymentIntentSucceeded = "payment_intent.succeeded";
    public const string PaymentIntentPaymentFailed = "payment_intent.payment_failed";
}
