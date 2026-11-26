using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public class UpdatePasswordAfterResetCommand(ApplicationUser user, string newPassword) : IRequest
{
    public ApplicationUser User { get; } = user;
    public string NewPassword { get; } = newPassword;
}
