using IdentityService.Features.Identity.Register.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.Register.Commands;

public class CreateEmailVerificationOtpCommand(Guid userId) : IRequest<GeneratedEmailVerificationOtp>
{
    public Guid UserId { get; } = userId;
}
