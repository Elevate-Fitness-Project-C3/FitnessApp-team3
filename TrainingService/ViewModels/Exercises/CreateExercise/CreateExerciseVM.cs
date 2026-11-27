namespace TrainingService.ViewModels.Exercises.CreateExercise
{
    public class CreateExerciseVM
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string TargetMuscleGroup { get; set; }
        public required int TrackingType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
