
using MediatR;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;
using TrainingService.Features.Exercises.ChangeExerciseStatus;
using TrainingService.Features.Exercises.CreateExercise;
using TrainingService.Features.Exercises.GetExercises;
using TrainingService.Features.Exercises.UpdateExercise;
using TrainingService.ViewModels.Exercises.CreateExercise;
using TrainingService.ViewModels.Exercises.GetExercises;
using TrainingService.ViewModels.Exercises.Mappers;
using TrainingService.ViewModels.Exercises.UpdateExercise;


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


        // group.MapGet("/", GetAllExercises);
        group.MapGet("/", async ( [AsParameters] GetExercisesRequestVM viewModel, ISender sender, CancellationToken cancellationToken) =>
        {

            var query = new GetExercisesQuery(viewModel.ToDto());

            var result = await sender.Send( query, cancellationToken);

            var response = EndpointResponse<PaginatedResult<GetExercisesDto>>.FromResult(result);

            return Results.Json( response,statusCode: result.StatusCode);
        })
        .WithName("GetAllExercises");


        // group.MapGet("/{id:guid}", DeactivateExerciseStatusById);
        group.MapPatch("/{id:guid}/status", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new ChangeExerciseStatusCommand(id);

            var result = await sender.Send(command, cancellationToken);

            var response = EndpointResponse<bool>.FromResult(result);

            return Results.Json( response, statusCode: result.StatusCode);
        })
        .WithName("ChangeExerciseStatus");



        // group.MapPut("/{id:guid}", UpdateExercise);

        group.MapPut("/{id:guid}", async (Guid id,UpdateExerciseVM viewModel, ISender sender, CancellationToken cancellationToken) =>
        {
            var dto = viewModel.ToDto();
            var command = new UpdateExerciseCommand(id, dto);

            var result = await sender.Send(command, cancellationToken);

            var response = EndpointResponse<bool>.FromResult(result);

            return Results.Json(
                response,
                statusCode: result.StatusCode);
        })
        .WithName("UpdateExercise");

        // group.MapDelete("/{id:guid}", DeleteExercise);

        return app;
    }
}
