using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Commands;

public record LockRefreshSessionCommand(int SessionId) : IRequest<bool>;
