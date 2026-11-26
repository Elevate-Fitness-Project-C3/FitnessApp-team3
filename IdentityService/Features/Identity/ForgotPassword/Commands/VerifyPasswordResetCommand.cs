using IdentityService.Features.Identity.ForgotPassword.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public class VerifyPasswordResetCommand(string email, string otp) : IRequest<PasswordResetTokenDto?>
{
    public string Email { get; } = email;
    public string Otp { get; } = otp;
}
