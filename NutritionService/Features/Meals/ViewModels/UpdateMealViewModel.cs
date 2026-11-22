using NutritionService.Domain.Enums;

namespace NutritionService.Features.Meals.ViewModels
{
    public sealed record UpdateMealViewModel(
        string Name,
        MealType MealType,
        int Calories,
        decimal ProteinGrams);
}
