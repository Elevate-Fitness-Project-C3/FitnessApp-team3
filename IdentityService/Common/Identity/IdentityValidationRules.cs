using System.Net.Mail;
using FluentValidation;

namespace IdentityService.Common.Identity;

internal static class IdentityValidationRules
{
    public static IRuleBuilderOptions<T, string> IdentityEmail<T>(this IRuleBuilderInitial<T, string> rule)
        => rule.Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .Must(email => email.Trim().Length <= 256).WithMessage("Email must not exceed 256 characters.")
            .Must(email => MailAddress.TryCreate(email.Trim(), out var address)
                && address.Address == email.Trim()
                && !email.Contains('\r') && !email.Contains('\n'))
            .WithMessage("Enter a valid email address.");

    public static IRuleBuilderOptions<T, string> IdentityPassword<T>(this IRuleBuilderInitial<T, string> rule)
        => rule.Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 128).WithMessage("Password must contain between 8 and 128 characters.");
}
