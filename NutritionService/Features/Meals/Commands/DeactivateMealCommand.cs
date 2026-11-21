using MediatR;
using NutritionService.Comman.Results;

namespace NutritionService.Features.Meals.Commands
{
    public sealed record DeactivateMealCommand(
        Guid MealId) : IRequest<Result>;   
}
