using IdentityService.Features.Identity.ForgotPassword.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public record CreatePasswordResetTokenCommand(Guid UserId) : IRequest<PasswordResetTokenDto>;
