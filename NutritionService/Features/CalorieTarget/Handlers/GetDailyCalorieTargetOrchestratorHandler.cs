using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Features.CalorieTarget.DTOs;
using NutritionService.Features.CalorieTarget.Enums;
using NutritionService.Features.CalorieTarget.Orchestrator;
using NutritionService.Features.CalorieTarget.Queries;
using NutritionService.Features.Preferences.Queries;

namespace NutritionService.Features.CalorieTarget.Handlers
{
    public class GetDailyCalorieTargetOrchestratorHandler :
        IRequestHandler<GetDailyCalorieTargetOrchestrator, Result<DailyCalorieTargetDto, TargetError>>
    {
        private readonly IMediator _mediator;
        public GetDailyCalorieTargetOrchestratorHandler(IMediator mediator)
             => _mediator = mediator;
        public async Task<Result<DailyCalorieTargetDto, TargetError>> Handle(GetDailyCalorieTargetOrchestrator request, CancellationToken ct)
        {
            var prefResult = await _mediator.Send(new GetMyNutritionPreferenceQuery(request.UserId), ct);
            if(prefResult.IsFailure || !prefResult.Value.IsConfigured||prefResult.Value.NutritionGoal is null)
            {
                return Result<DailyCalorieTargetDto, TargetError>.Failure(
                    TargetError.GoalNotSet,
                    "Nutrition goal has not been set for this user.");
            }

            var tdeeResult =await _mediator.Send(new GetUserTdeeQuery(request.UserId), ct);
            if(tdeeResult.Status == FitnessProfileTdeeStatus.ProfileIncomplete)
                return Result<DailyCalorieTargetDto, TargetError>.Failure(
                TargetError.ProfileIncomplete,
                tdeeResult.ErrorMessage ?? "User fitness profile is incomplete.");

            if(tdeeResult.Status == FitnessProfileTdeeStatus.ServiceUnavailable || tdeeResult.Tdee is null)
                return Result<DailyCalorieTargetDto, TargetError>.Failure(
                TargetError.ServiceUnavailable,
                tdeeResult.ErrorMessage ?? "Fitness profile service is currently unavailable.");

            var calorieTargetDto = await _mediator.Send(
            new CalculateDailyCalorieTargetQuery(prefResult.Value.NutritionGoal.Value, tdeeResult),ct);

            if (calorieTargetDto.IsOutdated)
            {
                return Result<DailyCalorieTargetDto, TargetError>
                    .Failure(
                        TargetError.TdeeOutdated,
                        "Your TDEE estimate is outdated. " +
                        "Please update your fitness profile.");
            }
            return Result<DailyCalorieTargetDto, TargetError>.Success(calorieTargetDto);
        }
    }
}
