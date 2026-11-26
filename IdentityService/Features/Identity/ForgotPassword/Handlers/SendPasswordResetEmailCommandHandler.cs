using IdentityService.Domain.Services;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Features.Shared.Emails;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class SendPasswordResetEmailCommandHandler(IEmailSender emailSender)
    : IRequestHandler<SendPasswordResetEmailCommand, EmailDeliveryStatus>
{
    public Task<EmailDeliveryStatus> Handle(SendPasswordResetEmailCommand command, CancellationToken cancellationToken)
    {
        var (plainText, html) = OtpEmailTemplate.Render(
            "Reset your password",
            "Enter this code to reset your FitnessApp password.",
            command.Challenge.Code,
            command.Challenge.ExpiresAt,
            "If you didn't request a password reset, you can ignore this email.");
        return emailSender.SendAsync(command.Email, "Reset your FitnessApp password", plainText, html, cancellationToken);
    }
}
