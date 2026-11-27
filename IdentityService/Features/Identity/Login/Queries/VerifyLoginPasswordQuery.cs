using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.Login.Queries;

public class VerifyLoginPasswordQuery(ApplicationUser? user, string password) : IRequest<bool>
{
    public ApplicationUser? User { get; } = user;
    public string Password { get; } = password;
}
