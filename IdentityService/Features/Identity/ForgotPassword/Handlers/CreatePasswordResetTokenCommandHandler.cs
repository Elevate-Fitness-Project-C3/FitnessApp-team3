using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Features.Identity.ForgotPassword.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class CreatePasswordResetTokenCommandHandler(IUnitOfWork unitOfWork, IOtpService otpService, TimeProvider timeProvider)
    : IRequestHandler<CreatePasswordResetTokenCommand, PasswordResetTokenDto>
{
    public Task<PasswordResetTokenDto> Handle(CreatePasswordResetTokenCommand command, CancellationToken cancellationToken)
    {
        var challengeId = Guid.NewGuid();
        var token = PasswordResetToken.Create(challengeId);
        var now = timeProvider.GetUtcNow();
        var expiresAt = now.AddMinutes(5);
        unitOfWork.GetRepository<OtpCode>().Add(new OtpCode
        {
            UserId = command.UserId,
            ChallengeId = challengeId,
            Purpose = OtpCode.PasswordResetTokenPurpose,
            CodeHash = PasswordResetToken.Hash(otpService, token),
            CreatedAt = now,
            ExpiresAt = expiresAt
        });
        return Task.FromResult(new PasswordResetTokenDto(token, expiresAt));
    }
}
