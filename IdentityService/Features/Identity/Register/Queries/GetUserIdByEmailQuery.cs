using MediatR;

namespace IdentityService.Features.Identity.Register.Queries;

public record GetUserIdByEmailQuery(string email) : IRequest<Guid?>;
