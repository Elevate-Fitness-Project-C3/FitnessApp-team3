namespace NutritionService.Features.Preferences.ViewModels
{
    public sealed record NutritionGoalViewModel(
         Guid UserId,
        bool IsConfigured,
        string? NutritionGoal,
        DateTime? UpdatedAt,
        string Message);
}
