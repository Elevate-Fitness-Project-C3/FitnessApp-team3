using NutritionService.Features.CalorieTarget.DTOs;

namespace NutritionService.Features.CalorieTarget.ViewModels
{
    public static class CalorieTargetMapping
    {
        public static DailyCalorieTargetViewModel ToViewModel(this DailyCalorieTargetDto dto)
        {
            return new DailyCalorieTargetViewModel(
                DailyCalorieTarget: dto.DailyCalorieTarget,
                Unit: dto.Unit,
                Tdee: dto.Tdee,
                NutritionGoal: dto.NutritionGoal,
                CalorieAdjustment: dto.CalorieAdjustment,
                MinimumFloorApplied: dto.MinimumFloorApplied,
                TdeeCalculatedAt: dto.TdeeCalculatedAt,
                IsOutdated: dto.IsOutdated,
                Disclaimer: dto.Disclaimer
            );
        }
    }
}
