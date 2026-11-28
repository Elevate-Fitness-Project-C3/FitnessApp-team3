using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.Queries;
using FitnessProfileService.Features.Shared;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.EndPoints
{
    public static class BodyMeasurementsEndpoints
    {
        public static void MapBodyMeasurements(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/body-measurements/latest", async (
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

                var result = await mediator.Send(
                    new GetLatestBodyMeasurementQuery(userId),
                    cancellationToken);

                return Results.Ok(
                    ApiResponse.Ok(
                        result,
                        "Latest body measurement retrieved successfully",
                        StatusCodes.Status200OK));
            })
            .WithTags("Body Measurements")
            .WithSummary("Get latest body measurement");

            app.MapGet("api/body-measurements/history", async (
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

                var result = await mediator.Send(
                    new GetBodyMeasurementsHistoryQuery(userId),
                    cancellationToken);

                return Results.Ok(
                    ApiResponse.Ok(
                        result,
                        "Body measurements history retrieved successfully",
                        StatusCodes.Status200OK));
            })
            .WithTags("Body Measurements")
            .WithSummary("Get body measurements history");
        }
    }
}
