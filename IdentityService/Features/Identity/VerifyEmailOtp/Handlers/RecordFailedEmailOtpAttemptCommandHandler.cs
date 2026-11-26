using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.VerifyEmailOtp.Commands;
using IdentityService.Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Handlers;

public class RecordFailedEmailOtpAttemptCommandHandler(
    IUnitOfWork unitOfWork,
    IOptions<OtpSettings> options,
    TimeProvider timeProvider) : IRequestHandler<RecordFailedEmailOtpAttemptCommand>
{
    public async Task Handle(RecordFailedEmailOtpAttemptCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            var now = timeProvider.GetUtcNow();
            var maxFailedAttempts = options.Value.MaxFailedAttempts;
            var userOtps = unitOfWork.GetRepository<OtpCode>().GetAll()
                .Where(otp => otp.UserId == request.UserId && otp.Purpose == OtpCode.EmailVerificationPurpose);

            await userOtps.Where(otp => otp.Id == request.OtpId && !otp.IsUsed
                    && otp.ExpiresAt > now && otp.FailedAttempts < maxFailedAttempts
                    && !userOtps.Any(newerOtp => newerOtp.Sequence > otp.Sequence))
                .ExecuteUpdateAsync(setters => setters.SetProperty(otp => otp.FailedAttempts, otp => otp.FailedAttempts + 1),
                    cancellationToken);
        }, cancellationToken);
    }
}
