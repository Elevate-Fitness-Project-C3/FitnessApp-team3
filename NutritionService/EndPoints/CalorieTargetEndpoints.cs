using MediatR;
using NutritionService.Features.CalorieTarget.Enums;
using NutritionService.Features.CalorieTarget.Orchestrator;
using NutritionService.Features.CalorieTarget.ViewModels;
using System.Runtime.CompilerServices;

namespace NutritionService.EndPoints
{
    public static class CalorieTargetEndpoints 
    {
        public static IEndpointRouteBuilder MapCalorieTargetEndpoints(this IEndpointRouteBuilder app )
        {
            var group = app.MapGroup("/api/nutrition/calorie-target")
                .WithTags("Calorie Target");
            group.MapGet("/", async ( Guid UserId ,IMediator mediator,CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDailyCalorieTargetOrchestrator(UserId),ct);
                if (result.IsSuccess)
                    return Results.Ok(result.Value!.ToViewModel());
                return result.ErrorCode switch
                {
                    TargetError.GoalNotSet => Results.UnprocessableEntity(new { error = result.Error }),
                    TargetError.ProfileIncomplete=>Results.UnprocessableEntity(new {error=result.Error}),
                    TargetError.ServiceUnavailable=> Results.Json(
                        new { error =result.Error},
                        statusCode:StatusCodes.Status503ServiceUnavailable),
                    _=>Results.BadRequest(new { error = result.Error})
                };
            });
            return app;
        }
    }
}
