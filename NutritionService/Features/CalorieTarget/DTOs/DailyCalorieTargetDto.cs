using NutritionService.Domain.Enums;

namespace NutritionService.Features.CalorieTarget.DTOs
{
    public sealed record DailyCalorieTargetDto(
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
