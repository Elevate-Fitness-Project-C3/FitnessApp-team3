using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public record RequestPasswordResetCommand(string Email) : IRequest<bool>;
