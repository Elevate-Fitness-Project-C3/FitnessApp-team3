using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.RefreshTokens.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.RefreshTokens.Handlers;

public class GetRefreshTokenQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetRefreshTokenQuery, RefreshToken?>
{
    public Task<RefreshToken?> Handle(GetRefreshTokenQuery query, CancellationToken cancellationToken)
    {
        return unitOfWork.GetRepository<RefreshToken>().GetAll().AsNoTracking()
            .SingleOrDefaultAsync(token => token.TokenHash == query.TokenHash, cancellationToken);
    }
}
