using System.Security.Cryptography;
using System.Text;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.ForgotPassword.Queries;
using IdentityService.Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class GetValidPasswordResetChallengeQueryHandler(
    IUnitOfWork unitOfWork, IOtpService otpService, IOptions<OtpSettings> options, TimeProvider timeProvider)
    : IRequestHandler<GetValidPasswordResetChallengeQuery, PasswordResetChallengeValidation>
{
    public async Task<PasswordResetChallengeValidation> Handle(
        GetValidPasswordResetChallengeQuery query, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var challenge = await unitOfWork.GetRepository<OtpCode>().GetAll().AsNoTracking()
            .Where(candidate => candidate.UserId == query.UserId && candidate.Purpose == OtpCode.PasswordResetPurpose)
            .OrderByDescending(candidate => candidate.Sequence)
            .FirstOrDefaultAsync(cancellationToken);
        if (challenge is null || challenge.IsUsed || challenge.ExpiresAt <= now
            || challenge.FailedAttempts >= options.Value.MaxFailedAttempts)
            return default;

        var hash = PasswordResetCode.Hash(otpService, challenge.ChallengeId, query.Otp);
        var isValid = CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(challenge.CodeHash), Encoding.UTF8.GetBytes(hash));
        return new PasswordResetChallengeValidation(challenge.Id, isValid);
    }
}
