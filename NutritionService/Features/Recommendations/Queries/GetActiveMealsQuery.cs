using MediatR;
using NutritionService.Domain.Enums;
using NutritionService.Features.Recommendations.DTOs;

namespace NutritionService.Features.Recommendations.Queries
{
    public record GetActiveMealsQuery(MealType? MealType = null) : IRequest<IReadOnlyList<ActiveMealDto>>;
}
