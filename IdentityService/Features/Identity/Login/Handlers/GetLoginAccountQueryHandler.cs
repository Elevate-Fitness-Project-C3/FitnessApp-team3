using IdentityService.Common.Identity;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Login.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.Login.Handlers;

public class GetLoginAccountQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetLoginAccountQuery, ApplicationUser?>
{
    public Task<ApplicationUser?> Handle(GetLoginAccountQuery query, CancellationToken cancellationToken)
    {
        var email = EmailNormalizer.Normalize(query.Email);
        return unitOfWork.GetRepository<ApplicationUser>().GetAll().AsNoTracking()
            .SingleOrDefaultAsync(user => user.NormalizedEmail == email, cancellationToken);
    }
}
