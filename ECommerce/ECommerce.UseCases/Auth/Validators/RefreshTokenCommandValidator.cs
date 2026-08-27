using ECommerce.UseCases.Auth.Commands;
using FluentValidation;

namespace ECommerce.UseCases.Auth.Validators;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token مطلوب.");
    }
}