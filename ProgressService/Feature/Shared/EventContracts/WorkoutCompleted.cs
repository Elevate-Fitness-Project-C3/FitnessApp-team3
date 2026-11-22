namespace ProgressService.Feature.Shared.EventContracts
{
    public class WorkoutCompleted
    {
        public Guid SessionId { get; init; }
        public Guid UserId { get; init; }
        public Guid ProgramId { get; init; }
        public Guid ProgramWorkoutId { get; init; }
        public DateTimeOffset CompletedAt { get; init; }
        public int Version { get; init; } = 1;
    }
}
