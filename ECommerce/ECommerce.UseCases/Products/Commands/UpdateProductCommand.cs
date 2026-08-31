using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.UseCases.Products.Commands;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price) : IRequest<Result>;

internal sealed class UpdateProductHandler(
    IRepository<Domain.Entities.Product> productRepo,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductCommand, Result>
{
    public async Task<Result> Handle(
        UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepo.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
            return Result.Failure(
                Error.NotFound("Product.NotFound", "المنتج غير موجود."));

        var updateResult = product.UpdateDetails(request.Name, request.Description, request.Price);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        productRepo.Update(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
