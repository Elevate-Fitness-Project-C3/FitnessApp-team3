using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.WorkoutPrograms;

namespace TrainingService.Features.ProgramWorkouts.GetProgramWorkouts {
    public record GetProgramWorkoutsQuery(Guid Guid) : IRequest<RequestResponse<List<GetProgramWorkoutDto>>>;
}
