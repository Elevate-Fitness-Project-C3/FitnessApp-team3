using TrainingService.Domain.Enums;

namespace TrainingService.Dtos.Exercises
{
    public class CreateExerciseDto
    {
        public required string Name { get; set; } 
        public required string Description { get; set; }
        public required string TargetMuscleGroup { get; set; }
        public required ExerciseTrackingType TrackingType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
