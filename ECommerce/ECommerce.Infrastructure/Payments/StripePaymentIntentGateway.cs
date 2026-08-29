using ECommerce.Domain.Common;
using ECommerce.UseCases.Payments.Contracts;
using Microsoft.Extensions.Options;
using Stripe;

namespace ECommerce.Infrastructure.Payments;

internal sealed class StripePaymentIntentGateway : IStripePaymentIntentGateway
{
    private readonly PaymentIntentService _paymentIntentService;
    private readonly StripeOptions _options;

    public StripePaymentIntentGateway(IOptions<StripeOptions> options)
    {
        _options = options.Value;
        StripeConfiguration.ApiKey = _options.SecretKey;
        _paymentIntentService = new PaymentIntentService();
    }

    public async Task<Result<string>> CreateAsync(
        long amountInMinorUnits,
        string currency,
        Guid orderId,
        CancellationToken ct = default)
    {
        try
        {
            var intent = await _paymentIntentService.CreateAsync(
                new PaymentIntentCreateOptions
                {
                    Amount = amountInMinorUnits,
                    Currency = currency,
                    PaymentMethodTypes = ["card"],
                    Metadata = new Dictionary<string, string>
                    {
                        ["orderId"] = orderId.ToString()
                    }
                },
                cancellationToken: ct);

            return Result.Success(intent.Id);
        }
        catch (StripeException ex)
        {
            return Result.Failure<string>(
                Error.Failure("Stripe.CreateFailed", ex.Message));
        }
    }

    public async Task<Result<string>> UpdateAmountAsync(
        string paymentIntentId,
        long amountInMinorUnits,
        CancellationToken ct = default)
    {
        try
        {
            var intent = await _paymentIntentService.UpdateAsync(
                paymentIntentId,
                new PaymentIntentUpdateOptions
                {
                    Amount = amountInMinorUnits
                },
                cancellationToken: ct);

            return Result.Success(intent.Id);
        }
        catch (StripeException ex)
        {
            return Result.Failure<string>(
                Error.Failure("Stripe.UpdateFailed", ex.Message));
        }
    }

    public Result<StripeWebhookEvent> ParseWebhook(string json, string signature)
    {
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, signature, _options.WebhookSecret);

            if (stripeEvent.Data.Object is not PaymentIntent intent)
            {
                return Result.Success(new StripeWebhookEvent(stripeEvent.Type, string.Empty));
            }

            return Result.Success(new StripeWebhookEvent(stripeEvent.Type, intent.Id));
        }
        catch (StripeException ex)
        {
            return Result.Failure<StripeWebhookEvent>(
                Error.Validation("Stripe.Webhook", ex.Message));
        }
    }
}
