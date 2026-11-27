using IdentityService.Common.Identity;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Register.Queries;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace IdentityService.Features.Identity.Register.Handlers;

public class GetUserIdByEmailQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserIdByEmailQuery, Guid?>
{
    public Task<Guid?> Handle(GetUserIdByEmailQuery query, CancellationToken cancellationToken)
    {
        var normalizedEmail = EmailNormalizer.Normalize(query.email);
        var userRepository = unitOfWork.GetRepository<ApplicationUser>();
        return userRepository.GetAll()
            .Where(user => user.NormalizedEmail == normalizedEmail)
            .Select(user => (Guid?)user.Id)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
