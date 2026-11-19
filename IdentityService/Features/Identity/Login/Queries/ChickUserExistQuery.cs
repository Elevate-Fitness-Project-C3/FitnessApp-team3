using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.Login.Queries
{
    public record ChickUserExistQuery(string userEmail) : IRequest<bool>;
    public class ChickUserExistQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChickUserExistQuery, bool>
    {
        public async Task<bool> Handle(ChickUserExistQuery request, CancellationToken cancellationToken)
        {
            var UserRepository = unitOfWork.GetRepository<ApplicationUser>();
            var user = await UserRepository.GetAll().SingleOrDefaultAsync(u => u.Email == request.userEmail, cancellationToken);
            if(user == null)
            {
                return false;
            }
            return true;
        }
    }

}
