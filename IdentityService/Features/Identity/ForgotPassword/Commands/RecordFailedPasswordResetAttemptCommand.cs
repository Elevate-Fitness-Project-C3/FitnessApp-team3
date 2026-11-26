using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public record RecordFailedPasswordResetAttemptCommand(Guid ChallengeId) : IRequest;
