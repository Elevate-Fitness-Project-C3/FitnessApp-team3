using MediatR;
using Microsoft.AspNetCore.Mvc;
using NutritionService.Features.Preferences.Commands;
using NutritionService.Features.Preferences.Queries;
using NutritionService.Features.Preferences.ViewModels;
using NutritionService.Mapping;
namespace NutritionService.EndPoints
{
    public static class NutritionPreferenceEndpoints
    {
        public static IEndpointRouteBuilder MapNutritionPreferenceEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/nutrition/preferences")
                            .WithTags("Nutrition Preferences");
            group.MapGet("/", async (Guid userId, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetMyNutritionPreferenceQuery(userId), ct);
                return result.IsSuccess
                ? Results.Ok(result.Value.ToViewModel())
                : Results.BadRequest(result.Error);
            });
            group.MapPut("/{userId:guid}", async (Guid userId, [FromBody] SetNutritionGoalViewModel vm, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new SetNutritionGoalCommand(userId, vm.NutritionGoal), ct);
                return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(result.Error);
            });
            return app;
        }
    }
}
