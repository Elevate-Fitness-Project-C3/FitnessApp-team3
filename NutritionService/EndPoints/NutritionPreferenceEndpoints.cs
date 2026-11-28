using MediatR;
using Microsoft.AspNetCore.Mvc;
using NutritionService.Comman.Auth;
using NutritionService.Features.Preferences.Commands;
using NutritionService.Features.Preferences.Queries;
using NutritionService.Features.Preferences.ViewModels;
using NutritionService.Mapping;
using System.Security.Claims;
namespace NutritionService.EndPoints
{
    public static class NutritionPreferenceEndpoints
    {
        public static IEndpointRouteBuilder MapNutritionPreferenceEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/nutrition/preferences")
                            .WithTags("Nutrition Preferences")
                            .RequireAuthorization();
            group.MapGet("/", (ClaimsPrincipal user, IMediator mediator, CancellationToken ct) => user.WithUserId(async userId =>
            {
                var result = await mediator.Send(new GetMyNutritionPreferenceQuery(userId), ct);
                return result.IsSuccess
                    ? Results.Ok(result.Value.ToViewModel())
                    : Results.BadRequest(result.Error);
            }));

            group.MapPut("/", (ClaimsPrincipal user, [FromBody] SetNutritionGoalViewModel vm, IMediator mediator, CancellationToken ct) =>
                 user.WithUserId(async userId =>
                  {
                      var result = await mediator.Send(new SetNutritionGoalCommand(userId, vm.NutritionGoal), ct);
                      return result.IsSuccess
                          ? Results.NoContent()
                          : Results.BadRequest(result.Error);
                  }));
            return app;
        }
    }
}
