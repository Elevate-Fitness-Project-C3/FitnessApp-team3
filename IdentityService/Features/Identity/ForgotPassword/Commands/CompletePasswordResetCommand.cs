using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public class CompletePasswordResetCommand(string resetToken, string newPassword) : IRequest<bool>
{
    public string ResetToken { get; } = resetToken;
    public string NewPassword { get; } = newPassword;
}
