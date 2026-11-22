using System.Net.Mail;
using FluentValidation;
using IdentityService.Features.Identity.Login.DTOs;

namespace IdentityService.Features.Identity.Login.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(request => request.Email).Cascade(CascadeMode.Stop)
            .NotEmpty().Must(email => email.Trim().Length <= 256
                && MailAddress.TryCreate(email.Trim(), out var address) && address.Address == email.Trim()
                && !email.Contains('\r') && !email.Contains('\n'))
            .WithMessage("Enter a valid email address.");

        RuleFor(request => request.Password).NotEmpty().MaximumLength(128);
    }
}
