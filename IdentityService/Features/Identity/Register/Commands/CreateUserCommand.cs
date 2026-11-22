using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.Register.Commands;

public class CreateUserCommand(string name, string email, string password) : IRequest<ApplicationUser>
{
    public string Name { get; } = name;
    public string Email { get; } = email;
    public string Password { get; } = password;
}
