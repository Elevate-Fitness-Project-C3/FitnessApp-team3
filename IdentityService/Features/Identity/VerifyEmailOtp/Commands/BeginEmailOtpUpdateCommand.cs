using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Commands;

public record BeginEmailOtpUpdateCommand(int UserId) : IRequest<bool>;
