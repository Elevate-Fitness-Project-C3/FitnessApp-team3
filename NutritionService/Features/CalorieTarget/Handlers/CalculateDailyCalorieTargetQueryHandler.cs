using MediatR;
using Microsoft.Extensions.Options;
using NutritionService.Domain.Enums;
using NutritionService.Features.CalorieTarget.DTOs;
using NutritionService.Features.CalorieTarget.Queries;
using NutritionService.Options;

namespace NutritionService.Features.CalorieTarget.Handlers
{
    public class CalculateDailyCalorieTargetQueryHandler : IRequestHandler<CalculateDailyCalorieTargetQuery, DailyCalorieTargetDto>
    {
        private readonly CalorieTargetOptions _options;
        public CalculateDailyCalorieTargetQueryHandler(IOptions<CalorieTargetOptions> options)
            => _options = options.Value;

        public Task<DailyCalorieTargetDto> Handle(CalculateDailyCalorieTargetQuery request, CancellationToken ct)
        {
            var tdee = request.TdeeData.Tdee!.Value;
            var goal = request.Goal;
            var plannedAdjustment = goal switch
            {
                NutritionGoal.LoseWeight =>
                   -Math.Abs(_options.GradualLossDeficit),
                NutritionGoal.GainMuscle =>
                    Math.Abs(_options.GradualGainSurplus),
                NutritionGoal.MaintainWeight => 0m,
                _ => 0m
            };
            var rawTarget = tdee + plannedAdjustment;
            var isFloorApplied = goal == NutritionGoal.LoseWeight && rawTarget < _options.MinimumDailyCalories;   // True 
            var finalTarget = isFloorApplied ? _options.MinimumDailyCalories : rawTarget;
            var actualAdjustment = finalTarget - tdee;
            var isOutdated = request.TdeeData.CalculatedAt.HasValue &&
                 (DateTime.UtcNow - request.TdeeData.CalculatedAt.Value).TotalDays > _options.MaxTdeeAgeDays;

            var dto = new DailyCalorieTargetDto(
            DailyCalorieTarget: Math.Round(finalTarget, 2),
            Unit: "kcal/day",
            Tdee: Math.Round(tdee, 2),
            NutritionGoal: goal,
            CalorieAdjustment: Math.Round(actualAdjustment, 2),
            MinimumFloorApplied: isFloorApplied,
            TdeeCalculatedAt: request.TdeeData.CalculatedAt,
            IsOutdated: isOutdated,
            Disclaimer: _options.Disclaimer);
            return Task.FromResult(dto);

        }
    }
}
