using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Commands;

public record RotateRefreshTokenCommand(RefreshToken Current, DateTimeOffset Now) : IRequest<string?>;
