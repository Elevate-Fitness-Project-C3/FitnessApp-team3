namespace IdentityService.Features.Identity.VerifyEmailOtp.ViewModels
{
    public record VerifyEmailOtpVM(
        string Email,
        string Otp
    );

}
