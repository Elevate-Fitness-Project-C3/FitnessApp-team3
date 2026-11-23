using NutritionService.Domain.Enums;

namespace NutritionService.Features.Meals.ViewModels
{
    public sealed record CreateMealViewModel(
        string Name,
        MealType MealType,
        int Calories,
        decimal ProteinGrams);
}
