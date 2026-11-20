using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Features.Preferences.DTOs;

namespace NutritionService.Features.Preferences.Queries
{
    public sealed record GetMyNutritionPreferenceQuery(Guid UserId) : IRequest<Result<NutritionGoalDto>>;

}
