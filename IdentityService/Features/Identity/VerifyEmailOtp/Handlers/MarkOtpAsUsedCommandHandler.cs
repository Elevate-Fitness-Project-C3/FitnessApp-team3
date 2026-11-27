using System.Security.Cryptography;
using System.Text;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.VerifyEmailOtp.Commands;
using IdentityService.Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Handlers;

public class MarkOtpAsUsedCommandHandler(
    IUnitOfWork unitOfWork,
    IOtpService otpService,
    IOptions<OtpSettings> options,
    TimeProvider timeProvider) : IRequestHandler<MarkOtpAsUsedCommand, bool>
{
    public async Task<bool> Handle(MarkOtpAsUsedCommand request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var maxFailedAttempts = options.Value.MaxFailedAttempts;
        var userOtps = unitOfWork.GetRepository<OtpCode>().GetAll()
            .Where(otp => otp.UserId == request.UserId && otp.Purpose == OtpCode.EmailVerificationPurpose);
        var eligibleOtps = userOtps.Where(otp => !otp.IsUsed && otp.FailedAttempts < maxFailedAttempts
            && !userOtps.Any(newerOtp => newerOtp.Sequence > otp.Sequence));

        var challenge = await eligibleOtps.Where(otp => otp.ExpiresAt > now)
            .Select(otp => new { otp.Id, otp.CodeHash })
            .SingleOrDefaultAsync(cancellationToken);
        if (challenge is null || !CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(challenge.CodeHash),
                Encoding.UTF8.GetBytes(otpService.HashCode(request.Otp))))
            return false;

        var consumedAt = timeProvider.GetUtcNow();
        var updated = await eligibleOtps.Where(otp => otp.Id == challenge.Id && otp.ExpiresAt > consumedAt)
            .ExecuteUpdateAsync(setters => setters.SetProperty(otp => otp.IsUsed, true), cancellationToken);

        return updated == 1;
    }
}
