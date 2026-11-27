using MediatR;
using TrainingService.Dtos.Shared;

namespace TrainingService.Features.Exercises.ChangeExerciseStatus
{
    public record ChangeExerciseStatusCommand(Guid ExerciseId) : IRequest<RequestResponse<bool>>;

}
