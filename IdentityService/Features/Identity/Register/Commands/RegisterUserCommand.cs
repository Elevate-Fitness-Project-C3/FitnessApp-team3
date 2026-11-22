using IdentityService.Features.Identity.Register.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.Register.Commands;

public class RegisterUserCommand(string name, string email, string password) : IRequest<RegisterResponseDto>
{
    public string Name { get; } = name;
    public string Email { get; } = email;
    public string Password { get; } = password;
}
