using IdentityService.Domain.Services;
using MediatR;

namespace IdentityService.Features.Identity.Register.Commands;

public class SendVerificationEmailCommand(string email, string code, DateTimeOffset expiresAt)
    : IRequest<EmailDeliveryStatus>
{
    public string Email { get; } = email;
    public string Code { get; } = code;
    public DateTimeOffset ExpiresAt { get; } = expiresAt;
}
