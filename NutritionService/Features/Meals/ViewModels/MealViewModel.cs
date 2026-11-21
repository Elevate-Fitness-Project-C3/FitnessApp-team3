using NutritionService.Domain.Enums;

namespace NutritionService.Features.Meals.ViewModels
{
    public sealed record MealViewModel(
        Guid MealId,
        string Name,
        MealType MealType,
        int Calories,
        string CalorieUnit,
        decimal ProteinGrams,
        string ProteinUnit,
        bool IsActive);
}
