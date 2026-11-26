using IdentityService.Common.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.Accounts.Commands;
using IdentityService.Features.Identity.Accounts.Queries;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Features.Identity.ForgotPassword.Queries;
using IdentityService.Features.Identity.Register.Queries;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Orchestrators;

public class SendPasswordResetOrchestrator(
    ISender sender, IUnitOfWork unitOfWork, TimeProvider timeProvider, ILogger<SendPasswordResetOrchestrator> logger)
    : IRequestHandler<SendPasswordResetCommand>
{
    public async Task Handle(SendPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var userId = await sender.Send(new GetUserIdByEmailQuery(command.Email), cancellationToken);
        if (userId is null)
            return;

        SendPasswordResetEmailCommand? email = null;
        await unitOfWork.ExecuteAsync(async () =>
        {
            if (!await sender.Send(new LockIdentityAccountCommand(userId.Value), cancellationToken))
                return;

            var user = await sender.Send(new GetIdentityAccountQuery(userId.Value), cancellationToken);
            if (user is null || !user.IsEnabled || user.NormalizedEmail != EmailNormalizer.Normalize(command.Email))
                return;

            var now = timeProvider.GetUtcNow();
            var latest = await sender.Send(new GetLatestPasswordResetRequestQuery(user.Id), cancellationToken);
            if (latest > now.AddMinutes(-1))
                return;

            await sender.Send(new InvalidatePasswordResetChallengesCommand(user.Id, now), cancellationToken);
            var challenge = await sender.Send(new CreatePasswordResetChallengeCommand(user.Id, now), cancellationToken);
            email = new SendPasswordResetEmailCommand(user.Email, challenge);
        }, cancellationToken);

        if (email is null)
            return;

        var delivery = await sender.Send(email, cancellationToken);
        if (delivery != EmailDeliveryStatus.Accepted)
            logger.LogWarning("Password reset email delivery could not be confirmed ({DeliveryStatus}).", delivery);
    }
}
