using IdentityService.Domain.Common;

namespace IdentityService.Domain.Entities.Identity;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }
    public required string Token { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
