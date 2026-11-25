using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.RefreshTokens.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.RefreshTokens.Handlers;

public class LockRefreshSessionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<LockRefreshSessionCommand, bool>
{
    public async Task<bool> Handle(LockRefreshSessionCommand command, CancellationToken cancellationToken)
    {
        // Updating the original token serializes writes until the orchestrator's transaction ends.
        var updated = await unitOfWork.GetRepository<RefreshToken>().GetAll()
            .Where(token => token.Id == command.SessionId && token.SessionRootTokenId == null)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(token => token.SessionVersion, token => token.SessionVersion + 1), cancellationToken);
        return updated == 1;
    }
}
