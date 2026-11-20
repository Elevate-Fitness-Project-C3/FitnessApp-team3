using FitnessProfileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessProfileService.Infrastructure.Persistence.Configurations
{
    public class BodyMeasurementConfiguration : IEntityTypeConfiguration<BodyMeasurement>
    {
        public void Configure(EntityTypeBuilder<BodyMeasurement> builder)
        {
            builder.ToTable("BodyMeasurements");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HeightCm)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.WeightKg)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.RecordedAt)
                .IsRequired();

            builder.HasIndex(x => new { x.UserId, x.RecordedAt });
        }
    }

}
