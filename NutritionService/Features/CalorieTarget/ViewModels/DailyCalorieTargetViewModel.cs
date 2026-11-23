using NutritionService.Domain.Enums;

namespace NutritionService.Features.CalorieTarget.ViewModels
{
    public sealed record DailyCalorieTargetViewModel(
        decimal DailyCalorieTarget,
        string Unit,
        decimal Tdee,
        NutritionGoal NutritionGoal,
        decimal CalorieAdjustment,
        bool MinimumFloorApplied,
        DateTime? TdeeCalculatedAt,
        bool IsOutdated,
        string Disclaimer);
    
}
