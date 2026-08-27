using ECommerce.Domain.Basket;
using ECommerce.UseCases.Basket.Commands;
using FluentValidation;

namespace ECommerce.UseCases.Basket.Validators;

public sealed class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
{
    public UpdateBasketCommandValidator()
    {
        RuleFor(x => x.Basket.Id)
            .NotEmpty()
            .WithMessage("معرف السلة مطلوب.");

        RuleFor(x => x.Basket.Items)
            .NotNull()
            .WithMessage("عناصر السلة مطلوبة.");

        RuleForEach(x => x.Basket.Items)
            .Must(item => item.ProductId != Guid.Empty)
            .WithMessage("المنتج مطلوب.")
            .Must(item => item.Price > 0)
            .WithMessage("سعر المنتج غير صالح.")
            .Must(item => item.Quantity > 0)
            .WithMessage("الكمية لازم تكون أكبر من صفر.");
    }
}