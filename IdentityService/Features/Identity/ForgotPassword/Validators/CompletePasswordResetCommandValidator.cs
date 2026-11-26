using FluentValidation;
using IdentityService.Common.Identity;
using IdentityService.Features.Identity.ForgotPassword.Commands;

namespace IdentityService.Features.Identity.ForgotPassword.Validators;

public class CompletePasswordResetCommandValidator : AbstractValidator<CompletePasswordResetCommand>
{
    public CompletePasswordResetCommandValidator()
    {
        RuleFor(command => command.NewPassword).IdentityPassword();
    }
}
