using MediatR;

namespace IdentityService.Features.Identity.Login.Commands;

public record CreateAccessTokenCommand(Guid UserId, string Role, DateTimeOffset IssuedAt) : IRequest<string>;
