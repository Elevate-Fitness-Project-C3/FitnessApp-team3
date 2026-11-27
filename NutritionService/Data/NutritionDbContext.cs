using MassTransit;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;
using System.Reflection;

namespace NutritionService.Data
{
    public sealed class NutritionDbContext : DbContext
    {
        public NutritionDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<NutritionPreference> NutritionPreferences => Set<NutritionPreference>();
        public DbSet<Meal> Meals => Set<Meal>();
        public DbSet<UserTdeeSnapshot> UserTdeeSnapshots => Set<UserTdeeSnapshot>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }

    }
}
