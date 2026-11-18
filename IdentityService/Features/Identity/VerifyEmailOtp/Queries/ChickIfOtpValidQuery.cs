using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Queries
{
    public record ChickIfOtpValidQuery(
        int UserId,
        string Otp
    ):IRequest<bool>;


}
