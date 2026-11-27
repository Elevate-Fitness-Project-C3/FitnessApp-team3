using NutritionService.Domain.Enums;

namespace NutritionService.Features.Recommendations.DTOs
{
    public sealed record ActiveMealDto(
        Guid MealId,
        string Name,
        MealType MealType,
        int Calories,
        decimal ProteinGrams);

}
