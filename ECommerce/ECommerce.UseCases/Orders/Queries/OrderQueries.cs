using ECommerce.Domain.Common;
using ECommerce.Domain.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Auth.Contracts;
using ECommerce.UseCases.Orders.Dtos;
using ECommerce.UseCases.Orders.Specifications;
using MapsterMapper;
using MediatR;

namespace ECommerce.UseCases.Orders.Queries;

public sealed record GetOrdersForUserQuery() : IRequest<Result<IReadOnlyList<OrderResponse>>>;

internal sealed class GetOrdersForUserQueryHandler(
    ICurrentUser currentUser,
    IRepository<Order> orderRepo,
    IMapper mapper)
    : IRequestHandler<GetOrdersForUserQuery, Result<IReadOnlyList<OrderResponse>>>
{
    public async Task<Result<IReadOnlyList<OrderResponse>>> Handle(
        GetOrdersForUserQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(currentUser.Email))
            return Result.Failure<IReadOnlyList<OrderResponse>>(
                Error.Unauthorized("Auth.Unauthorized", "يجب تسجيل الدخول أولاً."));

        var orders = await orderRepo.GetAllWithSpecAsync(
            new OrdersWithItemsAndDeliveryMethodForUserSpec(currentUser.Email).AddDetails(),
            cancellationToken);

        var response = orders.Select(o => mapper.Map<OrderResponse>(o)).ToList();

        return Result.Success<IReadOnlyList<OrderResponse>>(response);
    }
}

public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderResponse>>;

internal sealed class GetOrderByIdQueryHandler(
    ICurrentUser currentUser,
    IRepository<Order> orderRepo,
    IMapper mapper)
    : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(
        GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(currentUser.Email))
            return Result.Failure<OrderResponse>(
                Error.Unauthorized("Auth.Unauthorized", "يجب تسجيل الدخول أولاً."));

        var order = await orderRepo.GetEntityWithSpecAsync(
            new OrderWithItemsByUserSpec(request.OrderId, currentUser.Email).AddDetails(),
            cancellationToken);

        if (order is null)
            return Result.Failure<OrderResponse>(
                Error.NotFound("Order.NotFound", "الطلب غير موجود."));

        return Result.Success(mapper.Map<OrderResponse>(order));
    }
}