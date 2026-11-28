using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.WorkoutPrograms;

namespace TrainingService.Features.ProgramWorkouts.CreateWorkoutProgram {
    public record CreateWorkoutProgramCommand(AssignWorkoutToProgramDto RequestDto) : IRequest<RequestResponse<Guid>>;
}
