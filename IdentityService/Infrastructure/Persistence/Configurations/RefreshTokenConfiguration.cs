using IdentityService.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken");
        builder.HasKey(token => token.Id);
        builder.Property(token => token.Id).HasColumnName("TokenId");
        builder.Property(token => token.SessionVersion).HasDefaultValue(0L);
        builder.Property(token => token.TokenHash).HasMaxLength(64).IsUnicode(false).IsRequired();
        builder.HasIndex(token => token.TokenHash).IsUnique();
        builder.HasOne<RefreshToken>()
            .WithMany()
            .HasForeignKey(token => token.SessionRootTokenId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(token => token.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
