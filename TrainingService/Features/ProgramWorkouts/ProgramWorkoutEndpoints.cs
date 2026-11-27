using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;
using TrainingService.Features.ProgramWorkouts.AssignWorkoutToProgram;
using TrainingService.Features.TrainingPrograms.CreateTrainingProgram;
using TrainingService.Features.TrainingPrograms.GetTrainingProgramById;
using TrainingService.Features.TrainingPrograms.GetTrainingPrograms;
using TrainingService.ViewModels.TrainingProgram;

namespace TrainingService.Features.TrainingPrograms;

public static class ProgramWorkoutEndpoints {
    public static IEndpointRouteBuilder MapProgramWorkoutEndpoints(
        this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/api/program-workouts")
            .WithTags("ProgramWorkouts");


        group.MapPost("/{programId:guid}/workouts", async (Guid programId, AssignWorkoutToProgramVM viewModel, ISender sender, CancellationToken cancellationToken) => {
            var dto = viewModel.ToDto(programId);

            var command = new AssignWorkoutToProgramOrchestrator(dto);

            var result = await sender.Send(command, cancellationToken);

            var response = EndpointResponse<Guid>.FromResult(result);

            return Results.Json(response, statusCode: result.StatusCode);
        }).WithName("AssignWorkoutToProgram");







        return app;
    }
}