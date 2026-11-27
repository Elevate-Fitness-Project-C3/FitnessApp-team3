using IdentityService.Domain.Services;
using IdentityService.Features.Identity.Register.Commands;
using IdentityService.Features.Shared.Emails;
using MediatR;

namespace IdentityService.Features.Identity.Register.Handlers;

public class SendVerificationEmailHandler(IEmailSender emailSender)
    : IRequestHandler<SendVerificationEmailCommand, EmailDeliveryStatus>
{
    public Task<EmailDeliveryStatus> Handle(
        SendVerificationEmailCommand command, CancellationToken cancellationToken)
    {
        var (plainText, html) = OtpEmailTemplate.Render(
            "Verify your email address",
            "Enter this code to verify your email address in FitnessApp.",
            command.Code,
            command.ExpiresAt,
            "If you didn't request a verification code, you can ignore this email.");
        return emailSender.SendAsync(command.Email, "Verify your FitnessApp email", plainText, html, cancellationToken);
    }
}
