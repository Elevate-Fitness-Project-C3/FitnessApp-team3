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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(TrainingDbContext).Assembly);
        }
    }
}