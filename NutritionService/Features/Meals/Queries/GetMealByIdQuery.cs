using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Features.Meals.DTOs;

namespace NutritionService.Features.Meals.Queries
{
    public sealed record GetMealByIdQuery (
        Guid MealId) : IRequest<Result<MealDto>>;
}
