using ProgressService.Domains.Enums.CompletedWorkout;
using MediatR;

namespace ProgressService.Feature.CompletedWorkout.Command
{
    public record CreateCompletedWorkoutCommand(
        Guid SessionId, Guid UserId, Guid ProgramId,
        Guid ProgramWorkoutId, DateTimeOffset CompletedAt, int Version)
    : IRequest<RecordResult>;
}
