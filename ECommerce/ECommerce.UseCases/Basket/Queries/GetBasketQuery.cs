using ECommerce.Domain.Basket;
using ECommerce.Domain.Common;
using ECommerce.UseCases.Basket.Contracts;
using MediatR;

namespace ECommerce.UseCases.Basket.Queries;

public sealed record GetBasketQuery(string BasketId) : IRequest<Result<CustomerBasket>>;

internal sealed class GetBasketQueryHandler(IBasketRepository basketRepository)
    : IRequestHandler<GetBasketQuery, Result<CustomerBasket>>
{
    public async Task<Result<CustomerBasket>> Handle(
        GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasketAsync(request.BasketId, cancellationToken);

        if (basket is null)
            return Result.Failure<CustomerBasket>(
                Error.NotFound("Basket.NotFound", "السلة غير موجودة."));

        return Result.Success(basket);
    }
}