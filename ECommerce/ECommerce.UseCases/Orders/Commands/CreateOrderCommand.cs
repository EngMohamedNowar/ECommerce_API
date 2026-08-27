using ECommerce.Domain.Common;
using ECommerce.Domain.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Auth.Contracts;
using ECommerce.UseCases.Basket.Contracts;
using ECommerce.UseCases.Orders.Dtos;
using MapsterMapper;
using MediatR;

namespace ECommerce.UseCases.Orders.Commands;

public sealed record CreateOrderCommand(
    string BasketId,
    Guid DeliveryMethodId,
    AddressRequest ShippingAddress) : IRequest<Result<OrderResponse>>;

internal sealed class CreateOrderCommandHandler(
    ICurrentUser currentUser,
    IBasketRepository basketRepository,
    IRepository<DeliveryMethod> deliveryMethodRepo,
    IRepository<Order> orderRepo,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(
        CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(currentUser.Email))
            return Result.Failure<OrderResponse>(
                Error.Unauthorized("Auth.Unauthorized", "يجب تسجيل الدخول أولاً."));

        var basket = await basketRepository.GetBasketAsync(request.BasketId, cancellationToken);
        if (basket is null)
            return Result.Failure<OrderResponse>(
                Error.NotFound("Basket.NotFound", "السلة غير موجودة."));

        var deliveryMethod = await deliveryMethodRepo.GetByIdAsync(request.DeliveryMethodId, cancellationToken);
        if (deliveryMethod is null)
            return Result.Failure<OrderResponse>(
                Error.NotFound("DeliveryMethod.NotFound", "طريقة التوصيل غير موجودة."));

        var addressResult = Address.Create(
            request.ShippingAddress.FirstName,
            request.ShippingAddress.LastName,
            request.ShippingAddress.Street,
            request.ShippingAddress.City,
            request.ShippingAddress.State,
            request.ShippingAddress.ZipCode);

        if (addressResult.IsFailure)
            return Result.Failure<OrderResponse>(addressResult.Error);

        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var itemResult = OrderItem.Create(
                new ProductItemOrdered(item.ProductId, item.ProductName, item.PictureUrl),
                item.Price,
                item.Quantity);

            if (itemResult.IsFailure)
                return Result.Failure<OrderResponse>(itemResult.Error);

            items.Add(itemResult.Value);
        }

        var orderResult = Order.Create(
            currentUser.Email,
            addressResult.Value,
            deliveryMethod,
            items);

        if (orderResult.IsFailure)
            return Result.Failure<OrderResponse>(orderResult.Error);

        orderRepo.Add(orderResult.Value);

        basket.SetDeliveryMethod(deliveryMethod.Id, deliveryMethod.Price);
        await basketRepository.UpdateBasketAsync(basket, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await basketRepository.DeleteBasketAsync(request.BasketId, cancellationToken);

        var orderResponse = mapper.Map<OrderResponse>(orderResult.Value);

        return Result.Success(orderResponse);
    }
}