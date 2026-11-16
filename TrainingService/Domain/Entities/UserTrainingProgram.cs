using TrainingService.Domain.Common;
using TrainingService.Domain.Enums;

namespace TrainingService.Domain.Entities
{
    public class UserTrainingProgram : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { get; set; } = null!;

        public UserTrainingProgramStatus Status { get; set; }
            = UserTrainingProgramStatus.Active;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public DateTime? StoppedAt { get; set; }

        public ICollection<WorkoutSession> WorkoutSessions { get; set; }
            = new List<WorkoutSession>();

      
    }

}
