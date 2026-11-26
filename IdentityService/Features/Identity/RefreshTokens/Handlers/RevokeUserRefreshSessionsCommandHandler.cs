using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.RefreshTokens.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.RefreshTokens.Handlers;

public class RevokeUserRefreshSessionsCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RevokeUserRefreshSessionsCommand>
{
    public Task Handle(RevokeUserRefreshSessionsCommand command, CancellationToken cancellationToken)
        => unitOfWork.GetRepository<RefreshToken>().GetAll()
            .Where(token => token.UserId == command.UserId && token.RevokedAt == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(token => token.RevokedAt, command.Now), cancellationToken);
}
