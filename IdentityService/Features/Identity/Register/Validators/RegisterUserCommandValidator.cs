using System.Net.Mail;
using FluentValidation;
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

        RuleFor(command => command.Email).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .Must(email => email.Trim().Length <= 256).WithMessage("Email must not exceed 256 characters.")
            .Must(email => MailAddress.TryCreate(email.Trim(), out var address)
                && address.Address == email.Trim()
                && !email.Contains('\r') && !email.Contains('\n'))
            .WithMessage("Enter a valid email address.");

        RuleFor(command => command.Password).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 128).WithMessage("Password must contain between 8 and 128 characters.");
    }
}
