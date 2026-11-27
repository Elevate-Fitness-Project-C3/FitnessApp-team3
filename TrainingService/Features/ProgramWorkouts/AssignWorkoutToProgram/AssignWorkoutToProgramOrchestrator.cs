using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;

namespace TrainingService.Features.ProgramWorkouts.AssignWorkoutToProgram {

    public record AssignWorkoutToProgramOrchestrator(AssignWorkoutToProgramDto RequestDto) : IRequest<RequestResponse<Guid>>;
}
