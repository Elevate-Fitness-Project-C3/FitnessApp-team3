using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public record InvalidatePasswordResetChallengesCommand(Guid UserId, DateTimeOffset Now) : IRequest;
