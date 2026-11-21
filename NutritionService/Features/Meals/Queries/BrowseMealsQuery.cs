using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;
using NutritionService.Features.Meals.DTOs;

namespace NutritionService.Features.Meals.Queries
{
    public sealed record BrowseMealsQuery(
        string? Search, MealType?MealType) 
        : IRequest<Result<List<MealDto>>>;
}
