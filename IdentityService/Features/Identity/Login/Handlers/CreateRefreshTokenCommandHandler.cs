using System.Security.Cryptography;
using System.Text;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Login.Commands;
using IdentityService.Infrastructure.Settings;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Features.Identity.Login.Handlers;

public class CreateRefreshTokenCommandHandler(IUnitOfWork unitOfWork, JwtSettings settings)
    : IRequestHandler<CreateRefreshTokenCommand, (string Token, DateTimeOffset ExpiresAt)>
{
    public async Task<(string Token, DateTimeOffset ExpiresAt)> Handle(
        CreateRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var rawToken = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(32));
        // HTTP cookie dates have whole-second precision. Persist the same instant.
        var expiry = DateTimeOffset.FromUnixTimeSeconds(command.IssuedAt.AddDays(settings.RefreshLifetimeDays).ToUnixTimeSeconds());

        await unitOfWork.ExecuteAsync(() =>
        {
            unitOfWork.GetRepository<RefreshToken>().Add(new RefreshToken
            {
                UserId = command.UserId,
                TokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken))),
                ExpiresAt = expiry
            });
            return Task.CompletedTask;
        }, cancellationToken);

        return (rawToken, expiry);
    }
}
