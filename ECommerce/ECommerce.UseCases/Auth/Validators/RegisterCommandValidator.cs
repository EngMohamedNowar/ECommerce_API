using ECommerce.UseCases.Auth.Commands;
using FluentValidation;

namespace ECommerce.UseCases.Auth.Validators;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]").WithMessage("كلمة المرور لازم تحتوي على حرف كبير.")
            .Matches(@"[a-z]").WithMessage("كلمة المرور لازم تحتوي على حرف صغير.")
            .Matches(@"[0-9]").WithMessage("كلمة المرور لازم تحتوي على رقم.")
            .Matches(@"[^\w\d]").WithMessage("كلمة المرور لازم تحتوي على رمز خاص.");
    }
}