using IdentityService.Domain.Entities.Identity;
using IdentityService.Features.Identity.Login.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Features.Identity.Login.Handlers;

public class VerifyLoginPasswordQueryHandler(
    IPasswordHasher<ApplicationUser> passwordHasher,
    DummyLoginAccount dummyAccount) : IRequestHandler<VerifyLoginPasswordQuery, bool>
{
    public Task<bool> Handle(VerifyLoginPasswordQuery query, CancellationToken cancellationToken)
    {
        var user = query.User ?? dummyAccount.User;
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, query.Password);
        return Task.FromResult(query.User is not null && result != PasswordVerificationResult.Failed);
    }
}
