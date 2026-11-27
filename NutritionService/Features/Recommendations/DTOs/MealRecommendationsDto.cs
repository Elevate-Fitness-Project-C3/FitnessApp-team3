using NutritionService.Domain.Enums;

namespace NutritionService.Features.Recommendations.DTOs
{
    public sealed record MealRecommendationsDto(
        bool IsPersonalized,
        decimal? DailyCalorieTarget,
        string? TargetUnit,
        NutritionGoal? NutritionGoal,
        string? MissingDataReason,
        string Note,
        IReadOnlyList<MealRecommendationItemDto> Meals);
}
