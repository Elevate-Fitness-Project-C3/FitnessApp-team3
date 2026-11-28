using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.WorkoutPrograms;

namespace TrainingService.Features.ProgramWorkouts.AssignWorkoutToProgram {

    public record AssignWorkoutToProgramOrchestrator(AssignWorkoutToProgramDto RequestDto) : IRequest<RequestResponse<Guid>>;
}
