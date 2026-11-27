using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;

namespace TrainingService.Infrastructure.Context
{
    public class TrainingDbContext : DbContext
    {
        public TrainingDbContext(
            DbContextOptions<TrainingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<Workout> Workouts=> Set<Workout>();

        public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();

        public DbSet<TrainingProgram> TrainingPrograms => Set<TrainingProgram>();

        public DbSet<ProgramWorkout> ProgramWorkouts => Set<ProgramWorkout>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(TrainingDbContext).Assembly);
        }
    }
}