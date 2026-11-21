
using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Features.Exercises.CreateExercise;
using TrainingService.ViewModels.Exercises.CreateExercise;
using TrainingService.ViewModels.Exercises.Mappers;


namespace TrainingService.Features.Exercises;

public static class ExerciseEndpoints
{
    public static IEndpointRouteBuilder MapExerciseEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/exercises")
            .WithTags("Exercises");

        // Create Exercise
        group.MapPost("/", async ( CreateExerciseVM viewModel, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateExerciseCommand( viewModel.ToDto());

            var result = await sender.Send(command, cancellationToken);

            return Results.Json( EndpointResponse<Guid>.FromResult(result), statusCode: result.StatusCode);
        })
        .WithName("CreateExercise");

        // Later:
        // group.MapGet("/", GetAllExercises);
        // group.MapGet("/{id:guid}", GetExerciseById);
        // group.MapPut("/{id:guid}", UpdateExercise);
        // group.MapDelete("/{id:guid}", DeleteExercise);

        return app;
    }
}
