using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.VerifyEmailOtp.Queries;
using IdentityService.Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Handlers
{
    public class ChickIfOtpValidQueryHandler(
        IUnitOfWork unitOfWork,
        IOtpService otpService,
        IOptions<OtpSettings> options,
        TimeProvider timeProvider) : IRequestHandler<ChickIfOtpValidQuery, EmailOtpValidationResult>
    {
        public async Task<EmailOtpValidationResult> Handle(ChickIfOtpValidQuery request, CancellationToken cancellationToken)
        {
            var now = timeProvider.GetUtcNow();
            var challenge = await unitOfWork.GetRepository<OtpCode>().GetAll()
                .AsNoTracking()
                .Where(otp => otp.UserId == request.UserId && otp.Purpose == OtpCode.EmailVerificationPurpose)
                .OrderByDescending(otp => otp.Sequence)
                .Take(1)
                .FirstOrDefaultAsync(otp => otp.ExpiresAt > now, cancellationToken);
            if (challenge is null || challenge.IsUsed
                || challenge.FailedAttempts >= options.Value.MaxFailedAttempts)
                return default;

            var isValidOtp = CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(challenge.CodeHash),
                Encoding.UTF8.GetBytes(otpService.HashCode(request.Otp)));

            return new EmailOtpValidationResult(isValidOtp, isValidOtp ? null : challenge.Id);
        }
    }
}
