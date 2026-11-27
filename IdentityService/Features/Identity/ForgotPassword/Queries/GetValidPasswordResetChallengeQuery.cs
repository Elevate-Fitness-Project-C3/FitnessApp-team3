using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Queries;

public class GetValidPasswordResetChallengeQuery(Guid userId, string otp) : IRequest<PasswordResetChallengeValidation>
{
    public Guid UserId { get; } = userId;
    public string Otp { get; } = otp;
}

public readonly record struct PasswordResetChallengeValidation(Guid? ChallengeId, bool IsValid);
