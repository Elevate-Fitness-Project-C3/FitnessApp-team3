using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Commands;

public record RevokeRefreshSessionCommand(Guid SessionId, DateTimeOffset Now) : IRequest;
