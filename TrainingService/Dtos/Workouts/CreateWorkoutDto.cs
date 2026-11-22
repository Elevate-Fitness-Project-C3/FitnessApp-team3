namespace TrainingService.Dtos.Workouts
{
    public class CreateWorkoutDto
    {
        public required string Title { get; set; }
        public int Difficulty { get; set; }
        public int EstimatedDuration { get; set; }

        public List<CreateWorkoutExerciseDto> Exercises { get; set; } = new();
    }
}

