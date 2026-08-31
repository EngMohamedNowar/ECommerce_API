using ECommerce.Domain.Common;
using ECommerce.Domain.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Orders.Dtos;
using MapsterMapper;
using MediatR;

namespace ECommerce.UseCases.Orders.Queries;

public sealed record GetDeliveryMethodsQuery() : IRequest<Result<IReadOnlyList<DeliveryMethodResponse>>>;

internal sealed class GetDeliveryMethodsQueryHandler(
    IRepository<DeliveryMethod> deliveryMethodRepo,
    IMapper mapper)
    : IRequestHandler<GetDeliveryMethodsQuery, Result<IReadOnlyList<DeliveryMethodResponse>>>
{
    public async Task<Result<IReadOnlyList<DeliveryMethodResponse>>> Handle(
        GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
    {
        var methods = await deliveryMethodRepo.GetAllAsync(cancellationToken);

        var response = methods.Select(m => mapper.Map<DeliveryMethodResponse>(m)).ToList();

        return Result.Success<IReadOnlyList<DeliveryMethodResponse>>(response);
    }
}