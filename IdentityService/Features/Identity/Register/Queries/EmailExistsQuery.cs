using MediatR;

namespace IdentityService.Features.Identity.Register.Queries;

public class EmailExistsQuery(string normalizedEmail) : IRequest<bool>
{
    public string NormalizedEmail { get; } = normalizedEmail;
}
