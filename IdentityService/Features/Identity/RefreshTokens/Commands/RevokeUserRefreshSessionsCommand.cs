using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Commands;

public record RevokeUserRefreshSessionsCommand(Guid UserId, DateTimeOffset Now) : IRequest;
