using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingService.Domain.Entities;

namespace TrainingService.Infrastructure.Persistence.Configurations {

    public class TrainingProgramConfiguration : IEntityTypeConfiguration<TrainingProgram> {

        public void Configure(EntityTypeBuilder<TrainingProgram> builder) {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Description).HasMaxLength(500);

            builder.Property(x => x.RequiredLevel).IsRequired();

            builder.Property(x => x.Goal).IsRequired();

            builder.Property(x => x.AccessType).IsRequired();

            builder.Property(x => x.Status).IsRequired();

            builder.ToTable("TrainingPrograms", table => {   
                table.HasCheckConstraint(
                    "CK_TrainingProgram_DurationWeeks",
                    "DurationWeeks >= 1");

                table.HasCheckConstraint(
                    "CK_TrainingProgram_DaysPerWeek",
                    "DaysPerWeek BETWEEN 1 AND 7");
            });
        }
    }
}
