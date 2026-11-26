using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public record SendPasswordResetCommand(string Email) : IRequest;
