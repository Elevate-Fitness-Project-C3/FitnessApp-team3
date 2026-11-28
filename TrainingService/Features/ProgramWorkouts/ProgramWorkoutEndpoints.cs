using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.WorkoutPrograms;
using TrainingService.Features.ProgramWorkouts.AssignWorkoutToProgram;
using TrainingService.Features.ProgramWorkouts.GetProgramWorkouts;
using TrainingService.ViewModels.ProgramWorkouts;
using TrainingService.ViewModels.TrainingProgram;

namespace TrainingService.Features.TrainingPrograms;

public static class ProgramWorkoutEndpoints {
    public static IEndpointRouteBuilder MapProgramWorkoutEndpoints(
        this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/api/program-workouts")
            .WithTags("ProgramWorkouts");


        group.MapPost("/{programId:guid}/workouts", async (Guid programId, AssignWorkoutToProgramVM viewModel, ISender sender, CancellationToken cancellationToken) => {

            var dto = viewModel.toProgramDto(programId);

            var command = new AssignWorkoutToProgramOrchestrator(dto);

            var result = await sender.Send(command, cancellationToken);

            var response = EndpointResponse<Guid>.FromResult(result);

            return Results.Json(response, statusCode: result.StatusCode);
        }).WithName("AssignWorkoutToProgram");


        group.MapGet("/{programId:guid}/workouts", async (Guid programId, ISender sender, CancellationToken cancellationToken) => {

            var query = new GetProgramWorkoutsQuery(programId);
            var result = await sender.Send(query, cancellationToken);
            var response = EndpointResponse<List<GetProgramWorkoutDto>>.FromResult(result);
            return Results.Json(response, statusCode: result.StatusCode);
        }).WithName("GetProgramWorkouts");




        return app;
    }
}