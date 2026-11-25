using ProgressService.Domains.Entities;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
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
            // Match the existing outbox migration when using MassTransit 8.
            modelBuilder.AddOutboxMessageEntity(outbox =>
            {
                outbox.Metadata.RemoveIndex([outbox.Property(message => message.EnqueueTime).Metadata]);
                outbox.Metadata.RemoveIndex([outbox.Property(message => message.ExpirationTime).Metadata]);
            });
            modelBuilder.AddOutboxStateEntity(outbox =>
            {
                outbox.Metadata.RemoveIndex([outbox.Property(state => state.Created).Metadata]);
                outbox.Property<string>("BusName").HasMaxLength(256);
                outbox.HasIndex("BusName", nameof(OutboxState.Created));
            });

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
