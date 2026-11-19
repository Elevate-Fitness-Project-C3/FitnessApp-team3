using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.VerifyEmailOtp.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Handlers
{
    public class RevokeAllUnusedOtpsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RevokeAllUnusedOtpsCommand, bool>
    {
        public async Task<bool> Handle(RevokeAllUnusedOtpsCommand request, CancellationToken cancellationToken)
        {
            var otpRepository = unitOfWork.GetRepository<OtpCode>();
            await unitOfWork.ExecuteAsync(async () =>
            {
                var userOtps = await otpRepository.GetAll()
                    .Where(otp => otp.UserId == request.UserId && otp.Purpose == OtpCode.EmailVerificationPurpose && !otp.IsUsed)
                    .ToListAsync(cancellationToken);
                foreach (var otp in userOtps)
                    otp.IsUsed = true;
            }, cancellationToken);

            return true;
        }
    }
}
