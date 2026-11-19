using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands
{
    public record MarkOtpAsUsedCommand(
        int UserId,
        string Otp) : IRequest<bool>;

}
