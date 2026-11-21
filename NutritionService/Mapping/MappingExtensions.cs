using NutritionService.Features.Meals.DTOs;
using NutritionService.Features.Meals.ViewModels;
using NutritionService.Features.Preferences.DTOs;
using NutritionService.Features.Preferences.ViewModels;

namespace NutritionService.Mapping
{
    public static class MappingExtensions
    {
        public static NutritionGoalViewModel ToViewModel(this NutritionGoalDto dto)
         => new NutritionGoalViewModel(
             dto.UserId,
             dto.IsConfigured,
             dto.NutritionGoal?.ToString(),
             dto.UpdatedAt,
             dto.IsConfigured
             ? "Nutrition goal retrieved successfully."
             : "No nutrition goal set yet. Please select Maintain, GradualLoss, or GradualGain.");
        public static MealViewModel ToViewModel(this MealDto dto)
         => new(
             dto.MealId,
             dto.Name,
             dto.MealType,
             dto.Calories,
             dto.CalorieUnit,
             dto.ProteinGrams,
             dto.ProteinUnit,
             dto.IsActive);
    }
}
