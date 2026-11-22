namespace TrainingService.ViewModels.Exercises.UpdateExercise
{
    public class UpdateExerciseVM
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string TargetMuscleGroup { get; set; }
        public required int TrackingType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
