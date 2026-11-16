using TrainingService.Domain.Common;
using TrainingService.Domain.Enums;

namespace TrainingService.Domain.Entities
{
    public class Exercise : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TargetMuscleGroup { get; set; } = string.Empty;
        public ExerciseTrackingType TrackingType { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
            = new List<WorkoutExercise>();
    }
}

