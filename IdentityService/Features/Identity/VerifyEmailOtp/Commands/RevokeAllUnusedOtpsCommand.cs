using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands
{
    public record RevokeAllUnusedOtpsCommand(int userId) : IRequest<bool>
    {
        public int UserId { get; } = userId;
    }
}
