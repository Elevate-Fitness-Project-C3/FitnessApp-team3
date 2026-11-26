using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Queries
{
    public record ChickIfOtpValidQuery(
        Guid UserId,
        string Otp
    ):IRequest<EmailOtpValidationResult>;

    public readonly record struct EmailOtpValidationResult(bool IsValid, Guid? FailedOtpId);
}
