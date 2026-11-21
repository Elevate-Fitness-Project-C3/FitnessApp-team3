using MediatR;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;

namespace TrainingService.Features.Exercises.CreateExercise
{
    public record CreateExerciseCommand(CreateExerciseDto CreateExerciseDto) : IRequest<RequestResponse<bool>>;

}
