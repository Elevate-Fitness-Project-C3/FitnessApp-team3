using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.Register.Commands;
using IdentityService.Features.Identity.Register.DTOs;
using IdentityService.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.Register.Handlers;

public class CreateEmailVerificationOtpHandler(
    IUnitOfWork unitOfWork,
    IOtpService otpService,
    IOptions<OtpSettings> options,
    TimeProvider timeProvider) : IRequestHandler<CreateEmailVerificationOtpCommand, GeneratedEmailVerificationOtp>
{
    private const int MaxCodeGenerationAttempts = 10;

    public async Task<GeneratedEmailVerificationOtp> Handle(
        CreateEmailVerificationOtpCommand command, CancellationToken cancellationToken)
    {
        GeneratedEmailVerificationOtp verification = null!;
        await unitOfWork.ExecuteAsync(async () =>
        {
            var now = timeProvider.GetUtcNow();
            var repository = unitOfWork.GetRepository<OtpCode>();
            var previousHashes = await repository.GetAll()
                .Where(otp => otp.UserId == command.UserId && otp.Purpose == OtpCode.EmailVerificationPurpose
                    && otp.ExpiresAt > now)
                .Select(otp => otp.CodeHash)
                .ToListAsync(cancellationToken);
            var unexpiredCodeHashes = previousHashes.ToHashSet(StringComparer.Ordinal);

            for (var attempt = 0; attempt < MaxCodeGenerationAttempts; attempt++)
            {
                var code = otpService.GenerateCode();
                var codeHash = otpService.HashCode(code);
                if (unexpiredCodeHashes.Contains(codeHash))
                    continue;

                var challenge = new OtpCode
                {
                    ChallengeId = Guid.NewGuid(),
                    UserId = command.UserId,
                    CodeHash = codeHash,
                    Purpose = OtpCode.EmailVerificationPurpose,
                    CreatedAt = now,
                    ExpiresAt = now.AddMinutes(options.Value.ExpirationMinutes),
                    IsUsed = false,
                    FailedAttempts = 0
                };
                repository.Add(challenge);
                verification = new GeneratedEmailVerificationOtp(challenge, code);
                return;
            }

            throw new InvalidOperationException("Could not generate a distinct email verification code.");
        }, cancellationToken);
        return verification;
    }
}

