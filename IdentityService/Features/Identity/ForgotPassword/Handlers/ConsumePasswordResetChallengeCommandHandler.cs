using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class ConsumePasswordResetChallengeCommandHandler(
    IUnitOfWork unitOfWork, IOptions<OtpSettings> options, TimeProvider timeProvider)
    : IRequestHandler<ConsumePasswordResetChallengeCommand, bool>
{
    public async Task<bool> Handle(ConsumePasswordResetChallengeCommand command, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var maxFailedAttempts = options.Value.MaxFailedAttempts;
        var updated = await unitOfWork.GetRepository<OtpCode>().GetAll()
            .Where(challenge => challenge.Id == command.ChallengeId
                && challenge.Purpose == OtpCode.PasswordResetPurpose && !challenge.IsUsed
                && challenge.ExpiresAt > now && challenge.FailedAttempts < maxFailedAttempts)
            .ExecuteUpdateAsync(setters => setters.SetProperty(challenge => challenge.IsUsed, true), cancellationToken);
        return updated == 1;
    }
}
