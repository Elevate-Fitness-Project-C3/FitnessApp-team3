namespace ProgressService.Domains.Entities
{
    public class CompletedWorkoutRecord
    {
        private CompletedWorkoutRecord() { }
        public Guid Id { get; private set; }
        public Guid SessionId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid ProgramId { get; private set; }
        public Guid ProgramWorkoutId { get; private set; }
        public DateTimeOffset CompletedAtUtc { get; private set; }
        public DateTimeOffset ReceivedAtUtc { get; private set; }
        public int EventVersion { get; private set; }

        /// <summary>
        /// To Create an Instance of the record
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userId"></param>
        /// <param name="programId"></param>
        /// <param name="programWorkoutId"></param>
        /// <param name="completedAt"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static CompletedWorkoutRecord Create(
           Guid sessionId, Guid userId, Guid programId, Guid programWorkoutId,
           DateTimeOffset completedAt, int version) => new()
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId,
            UserId = userId,
            ProgramId = programId,
            ProgramWorkoutId = programWorkoutId,
            CompletedAtUtc = completedAt.ToUniversalTime(),
            ReceivedAtUtc = DateTimeOffset.UtcNow,
            EventVersion = version
        };
    }
}
