using MediatR;
using Microsoft.AspNetCore.Mvc;
using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;
using NutritionService.Features.Meals.Commands;
using NutritionService.Features.Meals.Queries;
using NutritionService.Features.Meals.ViewModels;
using NutritionService.Mapping;

namespace NutritionService.EndPoints
{
    public static class MealEndpoints
    {
        public static IEndpointRouteBuilder MapMealEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/nutrition/meals")
            .WithTags("Meals")
            .RequireAuthorization();

            group.MapGet("/", async (
                string? search,
                MealType? mealtype,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new BrowseMealsQuery(search, mealtype), ct);
                return result.IsSuccess
                 ? Results.Ok(result.Value
                     .Select(m => m.ToViewModel()))
                 : result.Error.ToHttpError();
            }); 

            group.MapGet("/{mealId:guid}", async (
            Guid mealId,
            IMediator mediator,
            CancellationToken ct) =>
            {
                var result = await mediator.Send(
                    new GetMealByIdQuery(mealId), ct);
                return result.IsSuccess
                    ? Results.Ok(result.Value.ToViewModel())
                    : result.Error.ToHttpError();
            });

            var adminGroup = group.MapGroup("")
                .RequireAuthorization(p=>p.RequireRole("Admin"));

            adminGroup.MapPost("/", async ([FromBody] CreateMealViewModel vm, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new CreateMealCommand(
                    vm.Name,
                    vm.MealType,
                    vm.Calories,
                    vm.ProteinGrams), ct);
                return result.IsSuccess ? Results.Created() : result.Error.ToHttpError();
            });
            adminGroup.MapPut("/{mealId:guid}", async (
                Guid mealId,
                [FromBody] UpdateMealViewModel vm,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(
                new UpdateMealCommand(
                    mealId,
                    vm.Name,
                    vm.MealType,
                    vm.Calories,
                    vm.ProteinGrams), ct);
                return result.IsSuccess
                ? Results.NoContent()
                : result.Error.ToHttpError();
            });
            adminGroup.MapPatch("/{mealId:guid}/deactivate", async (
                    Guid mealId,
                    IMediator mediator,
                    CancellationToken ct) =>
            {
                var result = await mediator.Send(
                new DeactivateMealCommand(mealId), ct);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.Error.ToHttpError();
            });
            return app;
        }
    }
}
