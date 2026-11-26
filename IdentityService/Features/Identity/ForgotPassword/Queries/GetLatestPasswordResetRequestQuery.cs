using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Queries;

public record GetLatestPasswordResetRequestQuery(Guid UserId) : IRequest<DateTimeOffset?>;
