using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;

namespace NutritionService.Data.Configurations
{
    public sealed class NutritionPreferenceConfiguration : IEntityTypeConfiguration<NutritionPreference>
    {
        public void Configure(EntityTypeBuilder<NutritionPreference> builder)
        {
            builder.ToTable("NutritionPreferences");
            builder.HasKey(p => p.UserId);
            builder.Property(p => p.UserId).ValueGeneratedNever();

            builder.Property(p => p.NutritionGoal)
                   .HasConversion<string>()
                   .HasMaxLength(30)
                   .IsRequired();
            builder.Property(p => p.UpdatedAt)
                   .IsRequired();
        }
    }
}
