using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands
{
    public record ActivateUserCommend(
        int UserId
    ):IRequest<bool>;
}
