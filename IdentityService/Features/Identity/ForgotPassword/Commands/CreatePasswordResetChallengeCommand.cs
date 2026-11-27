using IdentityService.Features.Identity.ForgotPassword.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public record CreatePasswordResetChallengeCommand(Guid UserId, DateTimeOffset Now)
    : IRequest<GeneratedPasswordResetChallenge>;
