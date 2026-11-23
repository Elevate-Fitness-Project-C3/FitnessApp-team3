using MediatR;

namespace IdentityService.Features.Identity.Login.Commands;

public record CreateAccessTokenCommand(int UserId, string Role, DateTimeOffset IssuedAt) : IRequest<string>;
