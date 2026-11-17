using IdentityService.Domain.Services;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Register.Commands;
using MediatR;

namespace IdentityService.Features.Identity.Register.Handlers;

public class SendVerificationEmailHandler(IEmailSender emailSender, IUnitOfWork unitOfWork)
    : IRequestHandler<SendVerificationEmailCommand, EmailDeliveryStatus>
{
    public async Task<EmailDeliveryStatus> Handle(
        SendVerificationEmailCommand command, CancellationToken cancellationToken)
    {
        var delivery = EmailDeliveryStatus.Unavailable;
        await unitOfWork.ExecuteAsync(async () =>
        {
            var body = $"Your FitnessApp verification code is {command.Code}.\n"
                + $"It expires at {command.ExpiresAt:yyyy-MM-dd HH:mm:ss} UTC.\n"
                + "If you did not register, you can ignore this email.";
            delivery = await emailSender.SendAsync(
                command.Email, "Verify your FitnessApp email", body, cancellationToken);
        }, cancellationToken);
        return delivery;
    }
}
