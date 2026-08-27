using ECommerce.UseCases.Products.Commands;
using FluentValidation;

namespace ECommerce.UseCases.Products.Validators;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("السعر لازم يكون أكبر من صفر.");

        RuleFor(x => x.BrandId)
            .NotEmpty()
            .WithMessage("الماركة مطلوبة.");

        RuleFor(x => x.TypeId)
            .NotEmpty()
            .WithMessage("النوع مطلوب.");
    }
}