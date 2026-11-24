using TrainingService.Domain.Common;
using TrainingService.Domain.Enums;

namespace TrainingService.Domain.Entities
{
    public class WorkoutSession : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid UserTrainingProgramId { get; set; }
        public UserTrainingProgram UserTrainingProgram { get; set; } = null!;

        public Guid ProgramWorkoutId { get; set; }
        public ProgramWorkout ProgramWorkout { get; set; } = null!;

        public WorkoutSessionStatus Status { get; set; }
            = WorkoutSessionStatus.InProgress;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public DateTime? CanceledAt { get; set; }

        public ICollection<SessionExercise> SessionExercises { get; set; }
            = new List<SessionExercise>();
    }

}
