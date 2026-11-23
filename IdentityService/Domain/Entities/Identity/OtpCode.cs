using IdentityService.Domain.Common;

namespace IdentityService.Domain.Entities.Identity;

public class OtpCode : BaseEntity
{
    public const string EmailVerificationPurpose = "EmailVerification";

    public int UserId { get; set; }
    public Guid ChallengeId { get; set; }
    public required string CodeHash { get; set; }
    public string Purpose { get; set; } = EmailVerificationPurpose;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public int FailedAttempts { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
