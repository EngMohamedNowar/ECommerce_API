using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.UseCases.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Result>;

internal sealed class DeleteProductHandler(
    IRepository<Domain.Entities.Product> productRepo,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(
        DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepo.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
            return Result.Failure(
                Error.NotFound("Product.NotFound", "المنتج غير موجود."));

        productRepo.Delete(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
