using MediatR;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;

namespace TrainingService.Features.Exercises.UpdateExercise
{
    public record UpdateExerciseCommand(Guid ExerciseId,UpdateExerciseDto UpdateExerciseDto) : IRequest<RequestResponse<bool>>;

}
