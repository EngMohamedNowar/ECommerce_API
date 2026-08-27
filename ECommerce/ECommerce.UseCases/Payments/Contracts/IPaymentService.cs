using ECommerce.Domain.Common;
using ECommerce.UseCases.Orders.Dtos;

namespace ECommerce.UseCases.Payments.Contracts;

public interface IPaymentService
{
    Task<Result<OrderResponse>> CreateOrUpdatePaymentIntentAsync(
        Guid orderId, string buyerEmail, CancellationToken ct = default);
}

public sealed class StripeOptions
{
    public const string SectionName = "Stripe";

    public string SecretKey { get; init; } = null!;
    public string PublishableKey { get; init; } = null!;
    public string WebhookSecret { get; init; } = null!;
    public string Currency { get; init; } = "usd";
}