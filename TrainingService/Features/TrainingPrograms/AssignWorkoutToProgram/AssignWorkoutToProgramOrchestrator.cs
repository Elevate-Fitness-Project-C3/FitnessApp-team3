using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;

namespace TrainingService.Features.TrainingPrograms.AssignWorkoutToProgram {

    public record AssignWorkoutToProgramOrchestrator(AssignWorkoutToProgramDto RequestDto) : IRequest<RequestResponse<Guid>>;
}
