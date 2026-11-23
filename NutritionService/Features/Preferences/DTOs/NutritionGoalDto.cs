using NutritionService.Domain.Enums;

namespace NutritionService.Features.Preferences.DTOs
{
    public sealed record NutritionGoalDto(
        Guid UserId,
        bool IsConfigured,
        NutritionGoal? NutritionGoal,
        DateTime? UpdatedAt);
}
