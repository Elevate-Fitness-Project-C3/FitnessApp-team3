using IdentityService.Domain.Entities.Identity;
using IdentityService.Features.Identity.Register.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.Register.Commands;

public class CreateEmailVerificationOtpCommand(ApplicationUser user) : IRequest<GeneratedEmailVerificationOtp>
{
    public ApplicationUser User { get; } = user;
}
