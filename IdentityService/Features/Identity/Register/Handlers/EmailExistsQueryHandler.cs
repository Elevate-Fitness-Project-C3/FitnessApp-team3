using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Register.Queries;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace IdentityService.Features.Identity.Register.Handlers;

public class EmailExistsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EmailExistsQuery, bool>
{
    public Task<bool> Handle(EmailExistsQuery query, CancellationToken cancellationToken)
        => unitOfWork.GetRepository<ApplicationUser>().GetAll()
            .AnyAsync(user => user.NormalizedEmail == query.NormalizedEmail, cancellationToken);
}
