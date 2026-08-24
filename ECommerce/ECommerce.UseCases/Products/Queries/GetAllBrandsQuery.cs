using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetAllBrandsQuery() : IRequest<Result<IReadOnlyList<GetAllBrandsResponse>>>;

internal sealed class GetAllBrandsHandler(IBrandQueryService brandQueryService)
    : IRequestHandler<GetAllBrandsQuery, Result<IReadOnlyList<GetAllBrandsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> Handle(
        GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await brandQueryService.GetAllBrandsAsync(cancellationToken);
        return Result.Success<IReadOnlyList<GetAllBrandsResponse>>(brands);
    }
}
