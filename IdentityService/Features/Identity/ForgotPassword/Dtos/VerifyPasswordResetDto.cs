namespace IdentityService.Features.Identity.ForgotPassword.DTOs;

public class VerifyPasswordResetDto
{
    public string Email { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}
