using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingService.Domain.Entities;

namespace TrainingService.Infrastructure.Persistence.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.TargetMuscleGroup)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TrackingType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .IsRequired();

        }
    }
}
