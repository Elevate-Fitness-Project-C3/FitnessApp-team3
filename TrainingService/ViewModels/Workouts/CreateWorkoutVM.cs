using TrainingService.Dtos.Workouts;

namespace TrainingService.ViewModels.Workouts
{
    public class CreateWorkoutVM
    {
        public required string Title { get; set; }
        public int Difficulty { get; set; }
        public int EstimatedDuration { get; set; }

        public List<CreateWorkoutExerciseVM> Exercises { get; set; } = new();
    }
}
