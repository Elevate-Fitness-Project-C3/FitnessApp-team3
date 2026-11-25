using MediatR;

namespace IdentityService.Features.Identity.Logout.Commands;

public record LogoutCommand(string? Token) : IRequest;
