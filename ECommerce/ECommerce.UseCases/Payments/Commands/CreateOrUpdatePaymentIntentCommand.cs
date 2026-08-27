using ECommerce.Domain.Common;
using ECommerce.UseCases.Orders.Dtos;
using ECommerce.UseCases.Payments.Contracts;
using MediatR;

namespace ECommerce.UseCases.Payments.Commands;

public sealed record CreateOrUpdatePaymentIntentCommand(
    Guid OrderId) : IRequest<Result<OrderResponse>>;

internal sealed class CreateOrUpdatePaymentIntentCommandHandler(
    IPaymentService paymentService,
    ICurrentUser currentUser)
    : IRequestHandler<CreateOrUpdatePaymentIntentCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(
        CreateOrUpdatePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(currentUser.Email))
            return Result.Failure<OrderResponse>(
                Error.Unauthorized("Auth.Unauthorized", "يجب تسجيل الدخول أولاً."));

        return await paymentService.CreateOrUpdatePaymentIntentAsync(
            request.OrderId, currentUser.Email, cancellationToken);
    }
}