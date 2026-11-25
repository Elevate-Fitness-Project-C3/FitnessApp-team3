using IdentityService.Domain.Common;

namespace IdentityService.Domain.Entities.Identity;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }
    public int? SessionRootTokenId { get; set; }
    public long SessionVersion { get; set; }
    public required string TokenHash { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? RotatedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }

    public int SessionId => SessionRootTokenId ?? Id;
    public ApplicationUser User { get; set; } = null!;
}
