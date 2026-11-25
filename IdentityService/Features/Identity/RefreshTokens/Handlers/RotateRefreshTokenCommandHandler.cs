using IdentityService.Common.Identity;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.RefreshTokens.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.RefreshTokens.Handlers;

public class RotateRefreshTokenCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RotateRefreshTokenCommand, string?>
{
    public async Task<string?> Handle(RotateRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var current = command.Current;
        string? replacementToken = null;
        await unitOfWork.ExecuteAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<RefreshToken>();
            var updated = await repository.GetAll()
                .Where(token => token.Id == current.Id && token.RotatedAt == null
                    && token.RevokedAt == null && token.ExpiresAt > command.Now)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(token => token.RotatedAt, command.Now)
                    .SetProperty(token => token.RevokedAt, command.Now), cancellationToken);
            if (updated != 1)
                return;

            var replacement = RefreshTokenValue.Create(current.UserId, current.ExpiresAt, current.SessionId);
            repository.Add(replacement.Record);
            replacementToken = replacement.Token;
        }, cancellationToken);

        return replacementToken;
    }
}
