using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class UpdatePasswordAfterResetCommandHandler(
    IUnitOfWork unitOfWork, IPasswordHasher<ApplicationUser> passwordHasher)
    : IRequestHandler<UpdatePasswordAfterResetCommand>
{
    public async Task Handle(UpdatePasswordAfterResetCommand command, CancellationToken cancellationToken)
    {
        var passwordHash = passwordHasher.HashPassword(command.User, command.NewPassword);
        var updated = await unitOfWork.GetRepository<ApplicationUser>().GetAll()
            .Where(user => user.Id == command.User.Id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(user => user.PasswordHash, passwordHash)
                .SetProperty(user => user.FailedLoginAttempts, 0)
                .SetProperty(user => user.LockoutEnd, (DateTimeOffset?)null), cancellationToken);
        if (updated != 1)
            throw new InvalidOperationException("Could not update the password for the reset challenge.");
    }
}
