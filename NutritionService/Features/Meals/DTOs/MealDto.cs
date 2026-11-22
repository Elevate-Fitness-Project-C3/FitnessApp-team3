using NutritionService.Domain.Enums;

namespace NutritionService.Features.Meals.DTOs
{
    public sealed record MealDto(
         Guid MealId,
        string Name,
        MealType MealType,
        int Calories,
        string CalorieUnit,
        decimal ProteinGrams,
        string ProteinUnit,
        bool IsActive);
}
