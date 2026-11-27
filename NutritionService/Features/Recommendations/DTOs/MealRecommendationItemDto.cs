using NutritionService.Domain.Enums;

namespace NutritionService.Features.Recommendations.DTOs
{
    public sealed record MealRecommendationItemDto(
        Guid MealId,
        string Name,
        MealType MealType,
        int Calories,
        decimal ProteinGrams,
        decimal? PercentOfDailyTarget,
        string Reason);
}
