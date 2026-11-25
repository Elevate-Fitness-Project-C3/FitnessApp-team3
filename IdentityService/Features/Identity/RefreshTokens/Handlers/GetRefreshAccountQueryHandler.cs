using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.RefreshTokens.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.RefreshTokens.Handlers;

public class GetRefreshAccountQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetRefreshAccountQuery, ApplicationUser?>
{
    public Task<ApplicationUser?> Handle(GetRefreshAccountQuery query, CancellationToken cancellationToken)
    {
        return unitOfWork.GetRepository<ApplicationUser>().GetAll().AsNoTracking()
            .SingleOrDefaultAsync(user => user.Id == query.UserId, cancellationToken);
    }
}
