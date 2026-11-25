using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.RefreshTokens.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.RefreshTokens.Handlers;

public class RevokeRefreshSessionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RevokeRefreshSessionCommand>
{
    public Task Handle(RevokeRefreshSessionCommand command, CancellationToken cancellationToken)
    {
        return unitOfWork.GetRepository<RefreshToken>().GetAll()
            .Where(token => (token.Id == command.SessionId || token.SessionRootTokenId == command.SessionId)
                && token.RotatedAt == null && token.RevokedAt == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(token => token.RevokedAt, command.Now), cancellationToken);
    }
}
