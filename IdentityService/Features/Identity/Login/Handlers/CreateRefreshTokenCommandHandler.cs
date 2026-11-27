using IdentityService.Common.Identity;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Login.Commands;
using IdentityService.Infrastructure.Settings;
using MediatR;

namespace IdentityService.Features.Identity.Login.Handlers;

public class CreateRefreshTokenCommandHandler(IUnitOfWork unitOfWork, JwtSettings settings)
    : IRequestHandler<CreateRefreshTokenCommand, (string Token, DateTimeOffset ExpiresAt)>
{
    public async Task<(string Token, DateTimeOffset ExpiresAt)> Handle(
        CreateRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        // HTTP cookie dates have whole-second precision. Persist the same instant.
        var expiry = DateTimeOffset.FromUnixTimeSeconds(command.IssuedAt.AddDays(settings.RefreshLifetimeDays).ToUnixTimeSeconds());
        var (rawToken, refreshToken) = RefreshTokenValue.Create(command.UserId, expiry);

        await unitOfWork.ExecuteAsync(() =>
        {
            unitOfWork.GetRepository<RefreshToken>().Add(refreshToken);
            return Task.CompletedTask;
        }, cancellationToken);

        return (rawToken, expiry);
    }
}
