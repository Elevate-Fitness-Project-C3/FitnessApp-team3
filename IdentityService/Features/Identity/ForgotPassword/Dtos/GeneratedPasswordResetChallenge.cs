namespace IdentityService.Features.Identity.ForgotPassword.DTOs;

public class GeneratedPasswordResetChallenge(string code, DateTimeOffset expiresAt)
{
    public string Code { get; } = code;
    public DateTimeOffset ExpiresAt { get; } = expiresAt;
}
