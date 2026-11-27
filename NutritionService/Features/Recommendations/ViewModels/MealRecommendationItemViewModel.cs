namespace NutritionService.Features.Recommendations.ViewModels
{
    public sealed record MealRecommendationItemViewModel(
        Guid MealId,
        string Name,
        string MealType,
        int Calories,
        decimal ProteinGrams,
        decimal? PercentOfDailyTarget,
        string Reason);
}
