using ECommerce.Domain.OrderAggregate;
using ECommerce.UseCases.Orders.Commands;
using FluentValidation;

namespace ECommerce.UseCases.Orders.Validators;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.BasketId)
            .NotEmpty()
            .WithMessage("معرف السلة مطلوب.");

        RuleFor(x => x.DeliveryMethodId)
            .NotEmpty()
            .WithMessage("طريقة التوصيل مطلوبة.");

        RuleFor(x => x.ShippingAddress.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ShippingAddress.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ShippingAddress.Street)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.ShippingAddress.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ShippingAddress.State)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ShippingAddress.ZipCode)
            .NotEmpty()
            .MaximumLength(20);
    }
}