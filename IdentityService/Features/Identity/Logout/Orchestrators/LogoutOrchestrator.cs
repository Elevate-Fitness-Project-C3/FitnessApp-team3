using IdentityService.Common.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Logout.Commands;
using IdentityService.Features.Identity.RefreshTokens.Commands;
using IdentityService.Features.Identity.RefreshTokens.Queries;
using MediatR;

namespace IdentityService.Features.Identity.Logout.Orchestrators;

public class LogoutOrchestrator(
    ISender sender,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        if (!RefreshTokenValue.IsValidFormat(command.Token))
            return;

        var presented = await sender.Send(new GetRefreshTokenQuery(RefreshTokenValue.Hash(command.Token!)), cancellationToken);
        if (presented is null)
            return;

        await unitOfWork.ExecuteAsync(async () =>
        {
            if (!await sender.Send(new LockRefreshSessionCommand(presented.SessionId), cancellationToken))
                return;

            await sender.Send(new RevokeRefreshSessionCommand(presented.SessionId, timeProvider.GetUtcNow()), cancellationToken);
        }, cancellationToken);
    }
}
