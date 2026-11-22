using IdentityService.Features.Identity.Register.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.Register.Commands;

public class CreateEmailVerificationOtpCommand(int userId) : IRequest<GeneratedEmailVerificationOtp>
{
    public int UserId { get; } = userId;
}
