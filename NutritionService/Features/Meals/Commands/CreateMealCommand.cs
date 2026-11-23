using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;

namespace NutritionService.Features.Meals.Commands
{
    public sealed record CreateMealCommand(
        string Name,
        MealType MealType,
        int Calories,
        decimal ProteinGrams) : IRequest<Result<Guid>>;
    
}
