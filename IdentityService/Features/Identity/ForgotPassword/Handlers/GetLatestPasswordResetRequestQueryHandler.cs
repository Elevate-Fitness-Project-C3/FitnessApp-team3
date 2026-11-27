using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.ForgotPassword.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class GetLatestPasswordResetRequestQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetLatestPasswordResetRequestQuery, DateTimeOffset?>
{
    public Task<DateTimeOffset?> Handle(GetLatestPasswordResetRequestQuery query, CancellationToken cancellationToken)
        => unitOfWork.GetRepository<OtpCode>().GetAll()
            .Where(challenge => challenge.UserId == query.UserId && challenge.Purpose == OtpCode.PasswordResetPurpose)
            .OrderByDescending(challenge => challenge.Sequence)
            .Select(challenge => (DateTimeOffset?)challenge.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
}
