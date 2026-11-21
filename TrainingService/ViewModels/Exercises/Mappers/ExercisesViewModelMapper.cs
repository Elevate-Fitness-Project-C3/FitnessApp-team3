using TrainingService.Dtos.Exercises;
using TrainingService.ViewModels.Exercises.CreateExercise;

namespace TrainingService.ViewModels.Exercises.Mappers
{
    public static class ExercisesViewModelMapper
    {
        public static CreateExerciseDto ToDto(this CreateExerciseVM viewModel)
        {
            return new CreateExerciseDto
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                TargetMuscleGroup = viewModel.TargetMuscleGroup,
                TrackingType = (Domain.Enums.ExerciseTrackingType)viewModel.TrackingType,
                IsActive = viewModel.IsActive
            };
        }
    }
}
