using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;

namespace NutritionService.Data.Configurations
{
    public class UserTdeeSnapshotConfiguration : IEntityTypeConfiguration<UserTdeeSnapshot>
    {
        public void Configure(EntityTypeBuilder<UserTdeeSnapshot> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.Tdee).HasPrecision(8, 2);
        }
    }
}
