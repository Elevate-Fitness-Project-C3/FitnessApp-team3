using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class ConsumePasswordResetTokenCommandHandler(IUnitOfWork unitOfWork, TimeProvider timeProvider)
    : IRequestHandler<ConsumePasswordResetTokenCommand, bool>
{
    public async Task<bool> Handle(ConsumePasswordResetTokenCommand command, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var updated = await unitOfWork.GetRepository<OtpCode>().GetAll()
            .Where(challenge => challenge.Id == command.ChallengeId
                && challenge.Purpose == OtpCode.PasswordResetTokenPurpose && !challenge.IsUsed
                && challenge.ExpiresAt > now)
            .ExecuteUpdateAsync(setters => setters.SetProperty(challenge => challenge.IsUsed, true), cancellationToken);
        return updated == 1;
    }
}
