using ECommerce.Domain.Common;
using ECommerce.UseCases.Orders.Dtos;

namespace ECommerce.UseCases.Payments.Contracts;

public interface IPaymentService
{
    Task<Result<OrderResponse>> CreateOrUpdatePaymentIntentAsync(
        Guid orderId, string buyerEmail, CancellationToken ct = default);

    Task<Result> ProcessWebhookAsync(
        string json, string signature, CancellationToken ct = default);
}

public interface IStripePaymentIntentGateway
{
    Task<Result<string>> CreateAsync(
        long amountInMinorUnits,
        string currency,
        Guid orderId,
        CancellationToken ct = default);

    Task<Result<string>> UpdateAmountAsync(
        string paymentIntentId,
        long amountInMinorUnits,
        CancellationToken ct = default);

    Result<StripeWebhookEvent> ParseWebhook(string json, string signature);
}

public sealed record StripeWebhookEvent(string Type, string PaymentIntentId);

public sealed class StripeOptions
{
    public const string SectionName = "Stripe";

    public string SecretKey { get; init; } = null!;
    public string PublishableKey { get; init; } = null!;
    public string WebhookSecret { get; init; } = null!;
    public string Currency { get; init; } = "usd";
}