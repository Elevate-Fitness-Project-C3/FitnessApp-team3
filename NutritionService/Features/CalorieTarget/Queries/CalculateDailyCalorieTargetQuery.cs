using MediatR;
using NutritionService.Domain.Enums;
using NutritionService.Features.CalorieTarget.DTOs;

namespace NutritionService.Features.CalorieTarget.Queries
{
    public sealed record CalculateDailyCalorieTargetQuery(NutritionGoal Goal, FitnessProfileTdeeDto TdeeData) :
        IRequest<DailyCalorieTargetDto>;
}
