using IdentityService.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Persistence.Configurations;

public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
{
    public void Configure(EntityTypeBuilder<OtpCode> builder)
    {
        builder.ToTable("OtpCode");
        builder.HasKey(otp => otp.Id);
        builder.Property(otp => otp.Id).HasColumnName("OtpId");
        builder.Property(otp => otp.Sequence).UseIdentityColumn();
        builder.Property(otp => otp.CodeHash).HasMaxLength(64).IsRequired();
        builder.Property(otp => otp.Purpose).HasMaxLength(32).IsRequired();
        builder.HasIndex(otp => otp.ChallengeId).IsUnique();
        builder.HasOne(otp => otp.User)
            .WithMany(user => user.OtpCodes)
            .HasForeignKey(otp => otp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
