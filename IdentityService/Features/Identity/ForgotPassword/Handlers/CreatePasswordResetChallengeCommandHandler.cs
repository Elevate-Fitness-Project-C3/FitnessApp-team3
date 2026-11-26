using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Features.Identity.ForgotPassword.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class CreatePasswordResetChallengeCommandHandler(IUnitOfWork unitOfWork, IOtpService otpService)
    : IRequestHandler<CreatePasswordResetChallengeCommand, GeneratedPasswordResetChallenge>
{
    private const int MaxCodeGenerationAttempts = 10;

    public async Task<GeneratedPasswordResetChallenge> Handle(
        CreatePasswordResetChallengeCommand command, CancellationToken cancellationToken)
    {
        var challengeId = Guid.NewGuid();
        var repository = unitOfWork.GetRepository<OtpCode>();
        var previousCodes = await repository.GetAll()
            .Where(challenge => challenge.UserId == command.UserId && challenge.Purpose == OtpCode.PasswordResetPurpose
                && challenge.ExpiresAt > command.Now)
            .Select(challenge => new { challenge.ChallengeId, challenge.CodeHash })
            .ToListAsync(cancellationToken);

        for (var attempt = 0; attempt < MaxCodeGenerationAttempts; attempt++)
        {
            var code = otpService.GenerateCode(PasswordResetCode.Length);
            if (previousCodes.Any(previous => previous.CodeHash == PasswordResetCode.Hash(otpService, previous.ChallengeId, code)))
                continue;

            var challenge = new OtpCode
            {
                ChallengeId = challengeId,
                UserId = command.UserId,
                Purpose = OtpCode.PasswordResetPurpose,
                CodeHash = PasswordResetCode.Hash(otpService, challengeId, code),
                CreatedAt = command.Now,
                ExpiresAt = command.Now.AddMinutes(10)
            };
            repository.Add(challenge);
            return new GeneratedPasswordResetChallenge(code, challenge.ExpiresAt);
        }

        throw new InvalidOperationException("Could not generate a distinct password reset code.");
    }
}
