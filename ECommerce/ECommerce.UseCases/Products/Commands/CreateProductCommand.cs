using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.UseCases.Products.Commands;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    string PictureUrl,
    decimal Price,
    Guid BrandId,
    Guid TypeId) : IRequest<Result<Guid>>;

internal sealed class CreateProductHandler(
    IRepository<Domain.Entities.Product> productRepo,
    IRepository<Domain.Entities.ProductBrand> brandRepo,
    IRepository<Domain.Entities.ProductType> typeRepo,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateProductCommand request, CancellationToken cancellationToken)
    {
        var brand = await brandRepo.GetByIdAsync(request.BrandId, cancellationToken);
        if (brand is null)
            return Result.Failure<Guid>(
                Error.NotFound("Brand.NotFound", "الماركة غير موجودة."));

        var type = await typeRepo.GetByIdAsync(request.TypeId, cancellationToken);
        if (type is null)
            return Result.Failure<Guid>(
                Error.NotFound("Type.NotFound", "النوع غير موجود."));

        var createResult = Domain.Entities.Product.Create(
            request.Name,
            request.Description,
            request.PictureUrl,
            request.Price,
            brand,
            type);

        if (createResult.IsFailure)
            return Result.Failure<Guid>(createResult.Error);

        productRepo.Add(createResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(createResult.Value.Id);
    }
}
