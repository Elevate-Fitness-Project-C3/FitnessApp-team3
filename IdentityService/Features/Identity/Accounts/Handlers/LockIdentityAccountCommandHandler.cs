using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Accounts.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.Accounts.Handlers;

public class LockIdentityAccountCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<LockIdentityAccountCommand, bool>
{
    public async Task<bool> Handle(LockIdentityAccountCommand command, CancellationToken cancellationToken)
    {
        // Hold the account write lock until the orchestrator commits.
        var updated = await unitOfWork.GetRepository<ApplicationUser>().GetAll()
            .Where(user => user.Id == command.UserId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(user => user.FailedLoginAttempts, user => user.FailedLoginAttempts), cancellationToken);
        return updated == 1;
    }
}
