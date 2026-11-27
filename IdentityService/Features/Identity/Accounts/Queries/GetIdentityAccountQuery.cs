using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.Accounts.Queries;

public record GetIdentityAccountQuery(Guid UserId) : IRequest<ApplicationUser?>;
