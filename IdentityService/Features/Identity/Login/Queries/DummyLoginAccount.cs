using System.Security.Cryptography;
using IdentityService.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Features.Identity.Login.Queries;

public class DummyLoginAccount
{
    public ApplicationUser User { get; } = new() { Email = string.Empty, Role = string.Empty, PasswordHash = string.Empty };

    public DummyLoginAccount(IPasswordHasher<ApplicationUser> passwordHasher)
    {
        User.PasswordHash = passwordHasher.HashPassword(User, Convert.ToHexString(RandomNumberGenerator.GetBytes(32)));
    }
}
