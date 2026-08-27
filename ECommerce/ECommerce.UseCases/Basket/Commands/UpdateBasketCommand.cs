using ECommerce.Domain.Basket;
using ECommerce.Domain.Common;
using ECommerce.UseCases.Basket.Contracts;
using MediatR;

namespace ECommerce.UseCases.Basket.Commands;

public sealed record UpdateBasketCommand(CustomerBasket Basket) : IRequest<Result<CustomerBasket>>;

internal sealed class UpdateBasketCommandHandler(IBasketRepository basketRepository)
    : IRequestHandler<UpdateBasketCommand, Result<CustomerBasket>>
{
    public async Task<Result<CustomerBasket>> Handle(
        UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Basket.Id))
            return Result.Failure<CustomerBasket>(
                Error.Validation("Basket.Id", "معرف السلة مطلوب."));

        var basket = await basketRepository.UpdateBasketAsync(request.Basket, cancellationToken);

        if (basket is null)
            return Result.Failure<CustomerBasket>(
                Error.Failure("Basket.SaveFailed", "فشل حفظ السلة."));

        return Result.Success(basket);
    }
}