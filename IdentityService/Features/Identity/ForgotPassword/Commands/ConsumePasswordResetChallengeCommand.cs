using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public record ConsumePasswordResetChallengeCommand(Guid ChallengeId) : IRequest<bool>;
