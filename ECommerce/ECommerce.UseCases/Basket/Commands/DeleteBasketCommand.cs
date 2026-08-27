using ECommerce.Domain.Common;
using ECommerce.UseCases.Basket.Contracts;
using MediatR;

namespace ECommerce.UseCases.Basket.Commands;

public sealed record DeleteBasketCommand(string BasketId) : IRequest<Result<string>>;

internal sealed class DeleteBasketCommandHandler(IBasketRepository basketRepository)
    : IRequestHandler<DeleteBasketCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var deleted = await basketRepository.DeleteBasketAsync(request.BasketId, cancellationToken);

        return deleted
            ? Result.Success(request.BasketId)
            : Result.Failure<string>(Error.NotFound("Basket.NotFound", "السلة غير موجودة."));
    }
}