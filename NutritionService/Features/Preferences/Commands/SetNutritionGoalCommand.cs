using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;

namespace NutritionService.Features.Preferences.Commands
{
    public sealed record SetNutritionGoalCommand(Guid UserID, NutritionGoal NutritionGoal) : IRequest<Result>;
}
