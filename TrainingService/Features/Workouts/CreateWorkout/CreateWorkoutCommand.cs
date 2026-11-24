using MediatR;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.Workouts;

namespace TrainingService.Features.Workouts.CreateWorkout
{
    public record CreateWorkoutCommand(CreateWorkoutDto CreateWorkoutDto) : IRequest<RequestResponse<Guid>>;

}
