using ECommerce.UseCases.Products.Commands;
using FluentValidation;

namespace ECommerce.UseCases.Products.Validators;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}