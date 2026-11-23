using TrainingService.Dtos.Workouts;

namespace TrainingService.ViewModels.Workouts;

public static class WorkoutsViewModelMapper
{
    public static CreateWorkoutDto ToDto(this CreateWorkoutVM viewModel)
    {
        return new CreateWorkoutDto
        {
            Title = viewModel.Title,
            Difficulty = viewModel.Difficulty,
            EstimatedDuration = viewModel.EstimatedDuration,

            Exercises = viewModel.Exercises
                .Select(x => new CreateWorkoutExerciseDto
                {
                    ExerciseId = x.ExerciseId,
                    Order = x.Order,
                    Sets = x.Sets,
                    Reps = x.Reps,
                    Duration = x.Duration,
                    Weight = x.Weight
                })
                .ToList()
        };
    }
}
