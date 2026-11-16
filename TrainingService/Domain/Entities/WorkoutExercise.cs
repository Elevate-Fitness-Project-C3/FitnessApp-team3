using TrainingService.Domain.Common;

namespace TrainingService.Domain.Entities
{

    public class WorkoutExercise : BaseEntity
    {
        public Guid WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;

        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;

        public int Order { get; set; }

        public int? TargetSets { get; set; }
        public int? TargetReps { get; set; }
        public int? TargetDurationSeconds { get; set; }
    }

}
