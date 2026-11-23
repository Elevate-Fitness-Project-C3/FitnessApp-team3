using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands;

public record RecordFailedEmailOtpAttemptCommand(int UserId, int OtpId) : IRequest;
