using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands;

public record RecordFailedEmailOtpAttemptCommand(Guid UserId, Guid OtpId) : IRequest;
