using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Commands;

public record LockRefreshSessionCommand(Guid SessionId) : IRequest<bool>;
