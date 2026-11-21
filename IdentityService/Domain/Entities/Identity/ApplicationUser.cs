using IdentityService.Domain.Common;

namespace IdentityService.Domain.Entities.Identity;

public class ApplicationUser : BaseEntity
{
    public string? Name { get; set; }
    public required string Email { get; set; }
    public string NormalizedEmail { get; set; } = string.Empty;
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
