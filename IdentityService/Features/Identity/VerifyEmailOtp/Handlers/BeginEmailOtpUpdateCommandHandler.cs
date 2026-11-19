using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.VerifyEmailOtp.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Handlers;

public class BeginEmailOtpUpdateCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<BeginEmailOtpUpdateCommand, bool>
{
    public async Task<bool> Handle(BeginEmailOtpUpdateCommand request, CancellationToken cancellationToken)
    {
        // Write the existing value so concurrent OTP requests wait for the caller's transaction.
        var updated = await unitOfWork.GetRepository<ApplicationUser>().GetAll()
            .Where(user => user.Id == request.UserId && !user.IsEmailVerified)
            .ExecuteUpdateAsync(setters => setters.SetProperty(user => user.IsEmailVerified, false), cancellationToken);

        return updated == 1;
    }
}
