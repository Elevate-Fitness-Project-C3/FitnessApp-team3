namespace IdentityService.Features.Identity.Register.DTOs;

public class RegisterResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
    public string EmailVerificationStatus { get; set; } = "Submitted";
}
