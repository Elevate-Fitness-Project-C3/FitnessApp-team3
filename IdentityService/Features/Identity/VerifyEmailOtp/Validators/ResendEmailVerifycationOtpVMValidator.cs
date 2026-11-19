using FluentValidation;
using IdentityService.Features.Identity.VerifyEmailOtp.ViewModels;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Validators
{
    public class ResendEmailVerifycationOtpVMValidator : AbstractValidator<ResendEmailVerifycationOtpVM>
    {
        public ResendEmailVerifycationOtpVMValidator()
        {
            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("User email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
