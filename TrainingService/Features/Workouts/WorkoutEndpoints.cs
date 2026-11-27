
using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.Workouts;
using TrainingService.Features.Workouts.CreateWorkout;
using TrainingService.ViewModels.Workouts;


namespace TrainingService.Features.Workouts;

public static class WorkoutEndpoints
{
    public static IEndpointRouteBuilder MapWorkoutEndpoints( this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workouts").WithTags("Workouts");

        group.MapPost("/", async (CreateWorkoutVM viewModel, ISender sender, CancellationToken cancellationToken) =>
        {
            var dto = viewModel.ToDto();
            var command = new CreateWorkoutCommand(dto);

            var result = await sender.Send(command, cancellationToken);

            var response =  EndpointResponse<Guid>.FromResult(result);

            return Results.Json(response, statusCode: result.StatusCode);
        })
        .WithName("CreateWorkout");

        return app;
    }
}
