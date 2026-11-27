using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands
{
    public record MarkOtpAsUsedCommand(
        Guid UserId,
        string Otp) : IRequest<bool>;

}
