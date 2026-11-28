using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingService.Domain.Entities;

namespace TrainingService.Infrastructure.Persistence.Configurations {
    public class ProgramWorkoutConfiguration :IEntityTypeConfiguration<ProgramWorkout> {
       

        public void Configure(EntityTypeBuilder<ProgramWorkout> builder) {

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.TrainingProgram)
            .WithMany(x => x.ProgramWorkouts)
            .HasForeignKey(x => x.TrainingProgramId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Workout)
                .WithMany(x => x.ProgramWorkouts)
                .HasForeignKey(x => x.WorkoutId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.TrainingProgramId,
                x.WeekNumber,
                x.DayNumber
            })
            .IsUnique();

            builder.ToTable("ProgramWorkouts", table =>
            {
                table.HasCheckConstraint(
                    "CK_ProgramWorkout_WeekNumber",
                    "WeekNumber >= 1");

                table.HasCheckConstraint(
                    "CK_ProgramWorkout_DayNumber",
                    "DayNumber >= 1");
            });
        }
    }
}
