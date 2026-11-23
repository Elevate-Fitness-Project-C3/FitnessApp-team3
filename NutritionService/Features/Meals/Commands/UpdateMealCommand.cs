using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;

namespace NutritionService.Features.Meals.Commands
{
    public record UpdateMealCommand(
        Guid MealId,
        string Name,
        MealType MealType,
        int Calories,
        decimal ProteinGrams) : IRequest<Result>;
}
