using FluentValidation;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Accounts.Commands;
using IdentityService.Features.Identity.Accounts.Queries;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Features.Identity.ForgotPassword.Queries;
using IdentityService.Features.Identity.RefreshTokens.Commands;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Orchestrators;

public class CompletePasswordResetOrchestrator(
    IValidator<CompletePasswordResetCommand> validator, ISender sender, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    : IRequestHandler<CompletePasswordResetCommand, bool>
{
    public async Task<bool> Handle(CompletePasswordResetCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var challenge = await sender.Send(new GetValidPasswordResetTokenQuery(command.ResetToken), cancellationToken);
        if (challenge is null)
            return false;

        var completed = false;
        await unitOfWork.ExecuteAsync(async () =>
        {
            if (!await sender.Send(new LockIdentityAccountCommand(challenge.UserId), cancellationToken))
                return;

            var user = await sender.Send(new GetIdentityAccountQuery(challenge.UserId), cancellationToken);
            if (user is null || !user.IsEnabled)
                return;

            if (!await sender.Send(new ConsumePasswordResetTokenCommand(challenge.Id), cancellationToken))
                return;

            await sender.Send(new UpdatePasswordAfterResetCommand(user, command.NewPassword), cancellationToken);
            await sender.Send(new RevokeUserRefreshSessionsCommand(user.Id, timeProvider.GetUtcNow()), cancellationToken);
            completed = true;
        }, cancellationToken);
        return completed;
    }
}
