using IdentityService.Features.Identity.Login.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.Login.Commands;

public class LoginCommand(string email, string password) : IRequest<LoginResult>
{
    public string Email { get; } = email;
    public string Password { get; } = password;
}
