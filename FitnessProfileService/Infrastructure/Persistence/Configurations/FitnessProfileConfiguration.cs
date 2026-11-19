using FitnessProfileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessProfileService.Infrastructure.Persistence.Configurations
{
    public class FitnessProfileConfiguration
        : IEntityTypeConfiguration<FitnessProfile>
    {
        public void Configure(EntityTypeBuilder<FitnessProfile> builder)
        {
            builder.ToTable("FitnessProfiles");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Gender)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.PrimaryGoal)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.PhysicalActivityLevel)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.TrainingExperienceLevel)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.AvailableDaysPerWeek)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasMany(x => x.BodyMeasurements)
                .WithOne(x => x.FitnessProfile)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
