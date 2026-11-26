using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public record ConsumePasswordResetTokenCommand(Guid ChallengeId) : IRequest<bool>;
