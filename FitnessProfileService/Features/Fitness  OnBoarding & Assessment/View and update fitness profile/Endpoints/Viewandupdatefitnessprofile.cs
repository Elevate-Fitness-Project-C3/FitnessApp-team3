using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Orchestrators;
using FitnessProfileService.Features.Shared;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Endpoints
{
    public static class Viewandupdatefitnessprofile
    {
        public static void MapViewFintnessProfile (this IEndpointRouteBuilder app)
        {
                app.MapGet("api/fitness-profile", async (
        IMediator mediator,
        CancellationToken cancellationToken) =>
            {
                var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

                var result = await mediator.Send(
                    new GetFitnessProfileOrchestrator(userId),
                    cancellationToken);

                return Results.Ok(
                    ApiResponse.Ok(
                        result,
                        "Fitness profile retrieved successfully",
                        StatusCodes.Status200OK));
            })
                .WithTags("Fitness Profile")
                .WithSummary("Get fitness profile");

        }
    }
}
