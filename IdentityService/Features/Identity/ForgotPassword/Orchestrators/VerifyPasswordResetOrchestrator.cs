using FluentValidation;
using IdentityService.Common.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Accounts.Commands;
using IdentityService.Features.Identity.Accounts.Queries;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Features.Identity.ForgotPassword.DTOs;
using IdentityService.Features.Identity.ForgotPassword.Queries;
using IdentityService.Features.Identity.Register.Queries;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Orchestrators;

public class VerifyPasswordResetOrchestrator(
    IValidator<VerifyPasswordResetCommand> validator, ISender sender, IUnitOfWork unitOfWork)
    : IRequestHandler<VerifyPasswordResetCommand, PasswordResetTokenDto?>
{
    public async Task<PasswordResetTokenDto?> Handle(VerifyPasswordResetCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var userId = await sender.Send(new GetUserIdByEmailQuery(command.Email), cancellationToken);
        if (userId is null)
            return null;

        PasswordResetTokenDto? token = null;
        await unitOfWork.ExecuteAsync(async () =>
        {
            if (!await sender.Send(new LockIdentityAccountCommand(userId.Value), cancellationToken))
                return;

            var user = await sender.Send(new GetIdentityAccountQuery(userId.Value), cancellationToken);
            if (user is null || !user.IsEnabled || user.NormalizedEmail != EmailNormalizer.Normalize(command.Email))
                return;

            var validation = await sender.Send(new GetValidPasswordResetChallengeQuery(user.Id, command.Otp), cancellationToken);
            if (validation.ChallengeId is not Guid challengeId)
                return;

            if (!validation.IsValid)
            {
                await sender.Send(new RecordFailedPasswordResetAttemptCommand(challengeId), cancellationToken);
                return;
            }

            if (!await sender.Send(new ConsumePasswordResetChallengeCommand(challengeId), cancellationToken))
                return;

            token = await sender.Send(new CreatePasswordResetTokenCommand(user.Id), cancellationToken);
        }, cancellationToken);
        return token;
    }
}
