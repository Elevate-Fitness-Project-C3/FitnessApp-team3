namespace NutritionService.Features.Recommendations.ViewModels
{
    public sealed record MealRecommendationsViewModel(
        bool IsPersonalized,
        decimal? DailyCalorieTarget,
        string? TargetUnit,
        string? NutritionGoal,
        string? MissingDataReason,
        string Note,
        IReadOnlyList<MealRecommendationItemViewModel>Meals);
}
