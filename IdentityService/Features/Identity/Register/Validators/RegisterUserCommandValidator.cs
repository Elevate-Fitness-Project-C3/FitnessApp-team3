using FluentValidation;
using IdentityService.Common.Identity;
using IdentityService.Features.Identity.Register.Commands;

namespace IdentityService.Features.Identity.Register.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Name).Cascade(CascadeMode.Stop)
            .NotEmpty().Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name is required.")
            .Must(name => name.Trim().Length <= 100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(command => command.Email).IdentityEmail();
        RuleFor(command => command.Password).IdentityPassword();
    }
}
