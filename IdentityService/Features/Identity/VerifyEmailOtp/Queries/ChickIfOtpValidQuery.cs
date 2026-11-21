using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Queries
{
    public record ChickIfOtpValidQuery(
        int UserId,
        string Otp
    ):IRequest<EmailOtpValidationResult>;

    public readonly record struct EmailOtpValidationResult(bool IsValid, int? FailedOtpId);
}
