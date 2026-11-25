using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Commands;

public record RevokeRefreshSessionCommand(int SessionId, DateTimeOffset Now) : IRequest;
