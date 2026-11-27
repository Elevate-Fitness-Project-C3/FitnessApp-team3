using MediatR;
using NutritionService.Comman.Auth;
using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;
using NutritionService.Features.Recommendations.Orchestrators;
using NutritionService.Mapping;
using System.Security.Claims;
namespace NutritionService.EndPoints;

public static class RecommendationEndpoints
{
    public static void MapRecommendationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/nutrition/recommendations")
            .WithTags("Recommendations")
            .RequireAuthorization();

         group.MapGet("/", (
            ClaimsPrincipal user,
            MealType? mealType,
            IMediator mediator,
            CancellationToken ct) =>
            user.WithUserId(async userId =>
            {
                var result = await mediator.Send(new GetMealRecommendationsOrchestrator(userId, mealType), ct);

                if (result.IsFailure)
                {
                    return result.Error.ToHttpError();
                }

                return Results.Ok(result.Value.ToViewModel());
            }));
    }
}