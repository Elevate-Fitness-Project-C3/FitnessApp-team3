using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.VerifyEmailOtp.Commands;
using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Handlers
{
    public class ActivateUserCommendHandler(IUnitOfWork unitOfWork) : IRequestHandler<ActivateUserCommend, bool>
    {
        public async Task<bool> Handle(ActivateUserCommend request, CancellationToken cancellationToken)
        {
            var userRepository = unitOfWork.GetRepository<ApplicationUser>();
            var activated = false;
            await unitOfWork.ExecuteAsync(async () =>
            {
                var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
                if (user is null)
                    return;

                user.IsEmailVerified = true;
                activated = true;
            }, cancellationToken);
            return activated;
        }
    }
}
