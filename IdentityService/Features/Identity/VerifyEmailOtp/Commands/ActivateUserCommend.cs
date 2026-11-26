using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands
{
    public record ActivateUserCommend(
        Guid UserId
    ):IRequest<bool>;
}
