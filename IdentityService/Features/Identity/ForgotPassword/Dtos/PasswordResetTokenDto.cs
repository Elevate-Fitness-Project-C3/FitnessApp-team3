namespace IdentityService.Features.Identity.ForgotPassword.DTOs;

public class PasswordResetTokenDto(string resetToken, DateTimeOffset expiresAt)
{
    public string ResetToken { get; } = resetToken;
    public DateTimeOffset ExpiresAt { get; } = expiresAt;
}
