using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Queries;

public class GetValidPasswordResetTokenQuery(string resetToken) : IRequest<OtpCode?>
{
    public string ResetToken { get; } = resetToken;
}
