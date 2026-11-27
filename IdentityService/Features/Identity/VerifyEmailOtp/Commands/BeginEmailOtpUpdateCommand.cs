using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands;

public record BeginEmailOtpUpdateCommand(Guid UserId) : IRequest<bool>;
