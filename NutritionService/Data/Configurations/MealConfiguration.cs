using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;

namespace NutritionService.Data.Configurations
{
    public sealed class MealConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.ToTable("Meals");
            builder.HasKey(m => m.MealId);
            builder.Property(m => m.MealId).ValueGeneratedNever();

            builder.Property(m => m.Name)
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(m => m.MealType)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();
            builder.Property(m => m.Calories)
                .IsRequired();
            builder.Property(m => m.ProteinGrams)
                .HasPrecision(6, 2)
                .IsRequired();
            builder.Property(m => m.IsActive)
                .IsRequired();
        }
    }
}
