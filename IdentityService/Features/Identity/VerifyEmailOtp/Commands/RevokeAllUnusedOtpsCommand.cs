using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands
{
    public record RevokeAllUnusedOtpsCommand(Guid userId) : IRequest<bool>
    {
        public Guid UserId { get; } = userId;
    }
}
