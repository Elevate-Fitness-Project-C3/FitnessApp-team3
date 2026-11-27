using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Queries;

public record GetRefreshTokenQuery(string TokenHash) : IRequest<RefreshToken?>;
