using System.Security.Cryptography;
using System.Text;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.ForgotPassword.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class GetValidPasswordResetTokenQueryHandler(
    IUnitOfWork unitOfWork, IOtpService otpService, TimeProvider timeProvider)
    : IRequestHandler<GetValidPasswordResetTokenQuery, OtpCode?>
{
    public async Task<OtpCode?> Handle(GetValidPasswordResetTokenQuery query, CancellationToken cancellationToken)
    {
        if (!PasswordResetToken.TryGetChallengeId(query.ResetToken, out var challengeId))
            return null;

        var now = timeProvider.GetUtcNow();
        var challenge = await unitOfWork.GetRepository<OtpCode>().GetAll().AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.ChallengeId == challengeId
                && candidate.Purpose == OtpCode.PasswordResetTokenPurpose
                && !candidate.IsUsed && candidate.ExpiresAt > now, cancellationToken);
        if (challenge is null || !CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(challenge.CodeHash),
                Encoding.UTF8.GetBytes(PasswordResetToken.Hash(otpService, query.ResetToken))))
            return null;

        return challenge;
    }
}
