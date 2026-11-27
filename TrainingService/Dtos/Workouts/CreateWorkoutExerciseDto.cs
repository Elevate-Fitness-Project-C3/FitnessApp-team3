namespace TrainingService.Dtos.Workouts
{
    public class CreateWorkoutExerciseDto
    {
        public Guid ExerciseId { get; set; }
        public int Order { get; set; }

        public int? Sets { get; set; }
        public int? Reps { get; set; }
        public int? Duration { get; set; }
        public decimal? Weight { get; set; }
    }
}
