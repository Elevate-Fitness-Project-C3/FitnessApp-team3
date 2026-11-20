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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
