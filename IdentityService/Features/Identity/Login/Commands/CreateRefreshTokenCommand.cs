using MediatR;

namespace IdentityService.Features.Identity.Login.Commands;

public record CreateRefreshTokenCommand(Guid UserId, DateTimeOffset IssuedAt)
    : IRequest<(string Token, DateTimeOffset ExpiresAt)>;
