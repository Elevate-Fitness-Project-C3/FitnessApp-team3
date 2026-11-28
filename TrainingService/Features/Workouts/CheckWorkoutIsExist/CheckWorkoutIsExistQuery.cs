using MediatR;
using TrainingService.Dtos.Shared;

namespace TrainingService.Features.Workouts.CheckWorkoutIsExist {
    public record CheckWorkoutIsExistQuery(Guid WorkoutId) : IRequest<RequestResponse<bool>>;
    
}
