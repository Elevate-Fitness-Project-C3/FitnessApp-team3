using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Accounts.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.Accounts.Handlers;

public class GetIdentityAccountQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetIdentityAccountQuery, ApplicationUser?>
{
    public Task<ApplicationUser?> Handle(GetIdentityAccountQuery query, CancellationToken cancellationToken)
    {
        return unitOfWork.GetRepository<ApplicationUser>().GetAll().AsNoTracking()
            .SingleOrDefaultAsync(user => user.Id == query.UserId, cancellationToken);
    }
}
