using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;
using NutritionService.Features.Recommendations.DTOs;

namespace NutritionService.Features.Recommendations.Orchestrators
{
    public record GetMealRecommendationsOrchestrator(Guid UserId, MealType? MealType = null)
        : IRequest<Result<MealRecommendationsDto>>;
}
