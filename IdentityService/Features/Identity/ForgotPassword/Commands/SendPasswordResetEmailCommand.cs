using IdentityService.Domain.Services;
using IdentityService.Features.Identity.ForgotPassword.DTOs;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Commands;

public class SendPasswordResetEmailCommand(string email, GeneratedPasswordResetChallenge challenge)
    : IRequest<EmailDeliveryStatus>
{
    public string Email { get; } = email;
    public GeneratedPasswordResetChallenge Challenge { get; } = challenge;
}
