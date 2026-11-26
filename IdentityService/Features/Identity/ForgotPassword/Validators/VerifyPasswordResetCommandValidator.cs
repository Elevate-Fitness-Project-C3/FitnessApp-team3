using FluentValidation;
using IdentityService.Common.Identity;
using IdentityService.Features.Identity.ForgotPassword.Commands;

namespace IdentityService.Features.Identity.ForgotPassword.Validators;

public class VerifyPasswordResetCommandValidator : AbstractValidator<VerifyPasswordResetCommand>
{
    public VerifyPasswordResetCommandValidator()
    {
        RuleFor(command => command.Email).IdentityEmail();
        RuleFor(command => command.Otp).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("The password reset code is required.")
            .Length(PasswordResetCode.Length)
            .Matches(@"\A[0-9]+\z").WithMessage("The password reset code must contain only digits.");
    }
}
