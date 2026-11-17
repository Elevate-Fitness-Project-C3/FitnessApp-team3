using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.Register.Commands;
using IdentityService.Features.Identity.Register.DTOs;
using IdentityService.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MediatR;

namespace IdentityService.Features.Identity.Register.Handlers;

public class CreateEmailVerificationOtpHandler(
    IUnitOfWork unitOfWork,
    IOtpService otpService,
    IOptions<OtpSettings> options,
    TimeProvider timeProvider) : IRequestHandler<CreateEmailVerificationOtpCommand, GeneratedEmailVerificationOtp>
{
    public async Task<GeneratedEmailVerificationOtp> Handle(
        CreateEmailVerificationOtpCommand command, CancellationToken cancellationToken)
    {
        GeneratedEmailVerificationOtp verification = null!;
        await unitOfWork.ExecuteAsync(() =>
        {
            var code = otpService.GenerateCode();
            var now = timeProvider.GetUtcNow();
            var challenge = new OtpCode
            {
                ChallengeId = Guid.NewGuid(),
                User = command.User,
                CodeHash = string.Empty,
                Purpose = OtpCode.EmailVerificationPurpose,
                CreatedAt = now,
                ExpiresAt = now.AddMinutes(options.Value.ExpirationMinutes),
                IsUsed = false,
                FailedAttempts = 0
            };
            challenge.CodeHash = otpService.HashCode(challenge.ChallengeId, challenge.Purpose, code);
            unitOfWork.GetRepository<OtpCode>().Add(challenge);
            verification = new GeneratedEmailVerificationOtp(challenge, code);
            return Task.CompletedTask;
        }, cancellationToken);
        return verification;
    }
}

