using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Features.CalorieTarget.DTOs;
using NutritionService.Features.CalorieTarget.Enums;

namespace NutritionService.Features.CalorieTarget.Orchestrator
{
    public sealed record GetDailyCalorieTargetOrchestrator(Guid UserId) : IRequest<Result<DailyCalorieTargetDto, TargetError>>;
}
