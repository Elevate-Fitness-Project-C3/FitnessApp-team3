using MediatR;

namespace IdentityService.Features.Identity.Accounts.Commands;

public record LockIdentityAccountCommand(Guid UserId) : IRequest<bool>;
