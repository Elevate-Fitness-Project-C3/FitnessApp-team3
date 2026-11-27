using IdentityService.Features.Identity.Login.Commands;
using IdentityService.Infrastructure.Settings;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Features.Identity.Login.Handlers;

public class CreateAccessTokenCommandHandler(JwtSettings settings, SigningCredentials credentials)
    : IRequestHandler<CreateAccessTokenCommand, string>
{
    public Task<string> Handle(CreateAccessTokenCommand command, CancellationToken cancellationToken)
    {
        var token = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = settings.Issuer,
            Audience = settings.Audience,
            IssuedAt = command.IssuedAt.UtcDateTime,
            NotBefore = command.IssuedAt.UtcDateTime,
            Expires = command.IssuedAt.AddMinutes(settings.AccessLifetimeMinutes).UtcDateTime,
            SigningCredentials = credentials,
            Claims = new Dictionary<string, object>
            {
                ["sub"] = command.UserId.ToString("D"),
                ["role"] = command.Role,
                ["jti"] = Guid.NewGuid().ToString("N")
            }
        });

        return Task.FromResult(token);
    }
}
