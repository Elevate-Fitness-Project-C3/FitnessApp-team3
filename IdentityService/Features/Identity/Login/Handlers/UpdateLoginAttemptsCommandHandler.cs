using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Login.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.Login.Handlers;

public class UpdateLoginAttemptsCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateLoginAttemptsCommand, bool>
{
    public async Task<bool> Handle(UpdateLoginAttemptsCommand command, CancellationToken cancellationToken)
    {
        var user = command.User;
        var previousAttempts = user.LockoutEnd.HasValue ? 0 : user.FailedLoginAttempts;
        var attempts = command.PasswordAccepted ? 0 : previousAttempts + 1;
        DateTimeOffset? lockoutEnd = attempts >= 5 ? command.Now.AddMinutes(15) : null;

        var updated = 0;
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Claim this version even when resetting an already-zero failure count.
            updated = await unitOfWork.GetRepository<ApplicationUser>().GetAll()
                .Where(account => account.Id == user.Id && account.RowVersion == user.RowVersion)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(account => account.FailedLoginAttempts, attempts)
                    .SetProperty(account => account.LockoutEnd, lockoutEnd), cancellationToken);
        }, cancellationToken);

        return updated == 1;
    }
}
