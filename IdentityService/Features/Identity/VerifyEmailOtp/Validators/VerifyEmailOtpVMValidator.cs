using FluentValidation;
using IdentityService.Features.Identity.VerifyEmailOtp.ViewModels;
using IdentityService.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Validators
{
    public class VerifyEmailOtpVMValidator :
        AbstractValidator<VerifyEmailOtpVM>
    {
        public VerifyEmailOtpVMValidator(IOptions<OtpSettings> options)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Otp)
                .NotEmpty()
                .Length(options.Value.Length)
                .Matches(@"\A[0-9]+\z").WithMessage("OTP must contain only digits.");
        }
    }
}
