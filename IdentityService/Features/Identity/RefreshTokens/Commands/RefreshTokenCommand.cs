using IdentityService.Features.Identity.RefreshTokens.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Commands;

public record RefreshTokenCommand(string? Token) : IRequest<RefreshTokenResult?>;
