using MediatR;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;
using TrainingService.Features.Exercises.GetExercises;
using TrainingService.Features.TrainingPrograms.AssignWorkoutToProgram;
using TrainingService.Features.TrainingPrograms.CreateTrainingProgram;
using TrainingService.Features.TrainingPrograms.GetTrainingProgramById;
using TrainingService.Features.TrainingPrograms.GetTrainingPrograms;
using TrainingService.ViewModels.TrainingProgram;

namespace TrainingService.Features.TrainingPrograms;

public static class TrainingProgramEndpoints {
    public static IEndpointRouteBuilder MapTrainingProgramEndpoints(
        this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/api/training-programs")
            .WithTags("TrainingPrograms");

        group.MapPost("/", async (
            CreateTrainingProgramVM viewModel,
            ISender sender,
            CancellationToken cancellationToken) => {
                var dto = viewModel.ToDto();

                var command = new GetTrainingProgramCommand(dto);

                var result = await sender.Send(
                    command, cancellationToken);

                var response = EndpointResponse<Guid>.FromResult(result);

                return Results.Json(
                    response,
                    statusCode: result.StatusCode);
            }).WithName("CreateTrainingProgram");
        //.RequireAuthorization(policy =>
        //    policy.RequireRole("Admin"));




        group.MapGet("/", async ([AsParameters] GetTrainingProgramRequestVM viewModel, ISender sender, CancellationToken cancellationToken) => {

            var query = new GetTrainingProgramQuery(viewModel.ToDto());

            var result = await sender.Send(query, cancellationToken);

            var response = EndpointResponse<PaginatedResult<GetTrainingProgramDto>>.FromResult(result);

            return Results.Json(response, statusCode: result.StatusCode);
        })
        .WithName("GetAllTrainingPrograms");



        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) => {
            var query = new GetTrainingProgramByIdQuery(id);

            var result = await sender.Send(query, cancellationToken);

            var response = EndpointResponse<GetTrainingProgramDto>.FromResult(result);

            return Results.Json(response, statusCode: result.StatusCode);
        }).WithName("GetTrainingProgramById");


        group.MapPost("/{programId:guid}/workouts", async (Guid programId, AssignWorkoutToProgramVM viewModel, ISender sender, CancellationToken cancellationToken) => {
            var dto = viewModel.ToDto(programId);

            var command = new AssignWorkoutToProgramOrchestrator(dto);

            var result = await sender.Send(command, cancellationToken);

            var response = EndpointResponse<Guid>.FromResult(result);

            return Results.Json(response, statusCode: result.StatusCode);
        })
                        .WithName("AssignWorkoutToProgram");


        return app;
    }
}