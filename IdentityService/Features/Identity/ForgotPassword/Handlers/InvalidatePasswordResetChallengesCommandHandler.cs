using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class InvalidatePasswordResetChallengesCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<InvalidatePasswordResetChallengesCommand>
{
    public Task Handle(InvalidatePasswordResetChallengesCommand command, CancellationToken cancellationToken)
        => unitOfWork.GetRepository<OtpCode>().GetAll()
            .Where(challenge => challenge.UserId == command.UserId
                && (challenge.Purpose == OtpCode.PasswordResetPurpose || challenge.Purpose == OtpCode.PasswordResetTokenPurpose)
                && !challenge.IsUsed && challenge.ExpiresAt > command.Now)
            .ExecuteUpdateAsync(setters => setters.SetProperty(challenge => challenge.IsUsed, true), cancellationToken);
}
