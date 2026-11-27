using System.Data;
using IdentityService.Common.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Accounts.Queries;
using IdentityService.Features.Identity.Login.Commands;
using IdentityService.Features.Identity.RefreshTokens.Commands;
using IdentityService.Features.Identity.RefreshTokens.DTOs;
using IdentityService.Features.Identity.RefreshTokens.Queries;
using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Orchestrators;

public class RefreshTokenOrchestrator(
    ISender sender,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    ILogger<RefreshTokenOrchestrator> logger) : IRequestHandler<RefreshTokenCommand, RefreshTokenResult?>
{
    public async Task<RefreshTokenResult?> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        if (!RefreshTokenValue.IsValidFormat(command.Token))
            return null;

        var query = new GetRefreshTokenQuery(RefreshTokenValue.Hash(command.Token!));
        var presented = await sender.Send(query, cancellationToken);
        if (presented is null)
            return null;

        RefreshTokenResult? result = null;
        var reused = false;
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Keep the account read lock through rotation so a password reset waits for the replacement.
            var user = await sender.Send(new GetIdentityAccountQuery(presented.UserId), cancellationToken);
            if (!await sender.Send(new LockRefreshSessionCommand(presented.SessionId), cancellationToken))
                return;

            var current = await sender.Send(query, cancellationToken);
            if (current is null)
                return;

            var now = timeProvider.GetUtcNow();
            if (current.RotatedAt is not null)
            {
                await sender.Send(new RevokeRefreshSessionCommand(current.SessionId, now), cancellationToken);
                reused = true;
                return;
            }

            if (current.RevokedAt is not null || current.ExpiresAt <= now)
                return;

            if (user is null || !user.IsEnabled || !user.IsEmailVerified)
                return;

            var replacement = await sender.Send(new RotateRefreshTokenCommand(current, now), cancellationToken);
            if (replacement is null)
                return;

            var accessToken = await sender.Send(new CreateAccessTokenCommand(user.Id, user.Role, now), cancellationToken);
            result = new RefreshTokenResult(accessToken, replacement, current.ExpiresAt);
        }, cancellationToken, IsolationLevel.RepeatableRead);

        if (reused)
            logger.LogWarning("Refresh token reuse detected for user {UserId}, session {SessionId}.",
                presented.UserId, presented.SessionId);

        return result;
    }
}
