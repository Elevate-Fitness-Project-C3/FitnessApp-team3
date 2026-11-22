using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.Login.Queries;

public record GetLoginAccountQuery(string Email) : IRequest<ApplicationUser?>;
