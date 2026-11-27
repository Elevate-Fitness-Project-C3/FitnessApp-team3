using TrainingService.Dtos.Exercises;
using TrainingService.ViewModels.Exercises.CreateExercise;
using TrainingService.ViewModels.Exercises.GetExercises;
using TrainingService.ViewModels.Exercises.UpdateExercise;

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


        public static GetExercisesRequestDto ToDto(this GetExercisesRequestVM viewModel)
        {
            return new GetExercisesRequestDto
            {
                PageIndex = viewModel.PageIndex ?? 1,
                PageSize = viewModel.PageSize ?? 10
            };
        }

        public static GetExercisesVM ToViewModel(this GetExercisesDto dto)
        {
            return new GetExercisesVM
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                TargetMuscleGroup = dto.TargetMuscleGroup,
                TrackingTypeID = dto.TrackingTypeID,
                TrackingType = dto.TrackingType,
                IsActive = dto.IsActive
            };
        }

        public static UpdateExerciseDto ToDto(this UpdateExerciseVM viewModel)
        {
            return new UpdateExerciseDto
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
