using ECommerce.Domain.Common;
using ECommerce.UseCases.Payments.Contracts;
using MediatR;

namespace ECommerce.UseCases.Payments.Commands;

public sealed record ProcessStripeWebhookCommand(
    string Json,
    string Signature) : IRequest<Result>;

internal sealed class ProcessStripeWebhookCommandHandler(IPaymentService paymentService)
    : IRequestHandler<ProcessStripeWebhookCommand, Result>
{
    public Task<Result> Handle(ProcessStripeWebhookCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Json))
            return Task.FromResult(Result.Failure(
                Error.Validation("Stripe.Webhook", "جسم الطلب فارغ.")));

        return paymentService.ProcessWebhookAsync(request.Json, request.Signature, cancellationToken);
    }
}
