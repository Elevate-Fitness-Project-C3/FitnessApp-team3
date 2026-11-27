using TrainingService.Domain.Common;

namespace TrainingService.Domain.Entities
{

    public class SessionExercise : BaseEntity
    {
        public Guid WorkoutSessionId { get; set; }
        public WorkoutSession WorkoutSession { get; set; } = null!;

        public Guid WorkoutExerciseId { get; set; }
        public WorkoutExercise WorkoutExercise { get; set; } = null!;

        public int? CompletedSets { get; set; }
        public int? CompletedReps { get; set; }
        public int? DurationSeconds { get; set; }
        public decimal? WeightKg { get; set; }

        public bool IsSkipped { get; set; }
    }

}
