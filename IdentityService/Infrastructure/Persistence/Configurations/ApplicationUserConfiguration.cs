using IdentityService.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("ApplicationUser");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.IsEnabled).HasDefaultValue(true);
        builder.Property(user => user.RowVersion).IsRowVersion();
        builder.Property(user => user.Id).HasColumnName("UserId");
        builder.Property(user => user.Name).HasMaxLength(100);
        builder.Property(user => user.Email).HasMaxLength(256).IsRequired();
        builder.Property(user => user.NormalizedEmail).HasMaxLength(256)
            .UseCollation("Latin1_General_100_BIN2").IsRequired();
        builder.HasIndex(user => user.NormalizedEmail).IsUnique()
            .HasDatabaseName(SqlServerExceptionHelper.NormalizedEmailIndex);
        builder.Property(user => user.PasswordHash).HasMaxLength(512).IsRequired();
        builder.Property(user => user.Role).HasMaxLength(32).IsRequired();
    }
}
