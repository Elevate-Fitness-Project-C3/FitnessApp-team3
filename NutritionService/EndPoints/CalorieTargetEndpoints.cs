using MediatR;
using NutritionService.Comman.Auth;
using NutritionService.Features.CalorieTarget.Enums;
using NutritionService.Features.CalorieTarget.Orchestrator;
using NutritionService.Features.CalorieTarget.ViewModels;
using System.Security.Claims;

namespace NutritionService.EndPoints
{
    public static class CalorieTargetEndpoints 
    {
        public static IEndpointRouteBuilder MapCalorieTargetEndpoints(this IEndpointRouteBuilder app )
        {
            var group = app.MapGroup("/api/nutrition/calorie-target")
                .WithTags("Calorie Target")
                .RequireAuthorization();
            group.MapGet("/", (ClaimsPrincipal user, IMediator mediator, CancellationToken ct) =>
                user.WithUserId(async userId =>
                {
                    var result = await mediator.Send(new GetDailyCalorieTargetOrchestrator(userId), ct);

                    if (result.IsSuccess)
                        return Results.Ok(result.Value!.ToViewModel());

                    return result.ErrorCode switch
                    {
                        TargetError.GoalNotSet
                            or TargetError.ProfileIncomplete
                            or TargetError.TdeeOutdated
                            => Results.UnprocessableEntity(
                                new { error = result.Error }),

                        TargetError.ServiceUnavailable
                            => Results.Json(
                                new { error = result.Error },
                                statusCode: StatusCodes.Status503ServiceUnavailable),

                        _ => Results.BadRequest(
                            new { error = result.Error })
                    };
                }));
            return app;
        }
    }
}
