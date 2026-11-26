using FluentValidation;
using IdentityService.Common.Identity;
using IdentityService.Features.Identity.ForgotPassword.Commands;

namespace IdentityService.Features.Identity.ForgotPassword.Validators;

public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
{
    public RequestPasswordResetCommandValidator()
    {
        RuleFor(command => command.Email).IdentityEmail();
    }
}
