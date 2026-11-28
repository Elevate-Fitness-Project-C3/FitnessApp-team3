using MediatR;
using Microsoft.Extensions.Options;
using NutritionService.Comman.Results;
using NutritionService.Features.CalorieTarget.Orchestrator;
using NutritionService.Features.Recommendations.DTOs;
using NutritionService.Features.Recommendations.Helpers;
using NutritionService.Features.Recommendations.Orchestrators;
using NutritionService.Features.Recommendations.Queries;
using NutritionService.Options;

namespace NutritionService.Features.Recommendations.Handlers;

public class GetMealRecommendationsOrchestratorHandler :
    IRequestHandler<GetMealRecommendationsOrchestrator, Result<MealRecommendationsDto>>
{
    private readonly MealRecommendationOptions _options;
    private readonly IMediator _mediator;

    public GetMealRecommendationsOrchestratorHandler(
        IOptions<MealRecommendationOptions> options,
        IMediator mediator)
    {
        _options = options.Value;
        _mediator = mediator;
    }

    public async Task<Result<MealRecommendationsDto>> Handle(
        GetMealRecommendationsOrchestrator request,
        CancellationToken ct)
    {
        var meals = await _mediator.Send(
            new GetActiveMealsQuery(request.MealType), ct);

        var targetResult = await _mediator.Send(
            new GetDailyCalorieTargetOrchestrator(
                request.UserId), ct);

        var target = targetResult.IsSuccess
            ? targetResult.Value
            : null;

        var dto = MealRecommendationRanker.Build(
            meals,
            target,
            targetResult.ErrorCode,
            targetResult.Error,
            _options);

        return Result.Success(dto);
    }
}