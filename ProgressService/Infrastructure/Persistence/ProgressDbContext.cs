using ProgressService.Domains.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ProgressService.Infrastructure.Persistence
{
    public class ProgressDbContext : DbContext
    {
        public ProgressDbContext(DbContextOptions<ProgressDbContext> options):base(options)
        {
        }

        public DbSet<CompletedWorkoutRecord> CompletedWorkoutRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            modelBuilder.Entity< CompletedWorkoutRecord > (entity =>
            {
                entity.ToTable("CompletedWorkoutRecords");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SessionId).IsUnique();
                entity.HasIndex(e => new { e.UserId, e.CompletedAtUtc, e.Id })
                    .IsDescending(false, true, true);
            });
        }
    }
}
