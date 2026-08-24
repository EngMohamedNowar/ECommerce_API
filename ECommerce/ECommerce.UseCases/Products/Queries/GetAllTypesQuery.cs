using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetAllTypesQuery() : IRequest<Result<IReadOnlyList<GetAllTypesResponse>>>;

internal sealed class GetAllTypesHandler(ITypeQueryService typeQueryService)
    : IRequestHandler<GetAllTypesQuery, Result<IReadOnlyList<GetAllTypesResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> Handle(
        GetAllTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await typeQueryService.GetAllTypesAsync(cancellationToken);
        return Result.Success<IReadOnlyList<GetAllTypesResponse>>(types);
    }
}
