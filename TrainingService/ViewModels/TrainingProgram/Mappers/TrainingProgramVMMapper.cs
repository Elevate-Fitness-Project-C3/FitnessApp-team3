using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.TrainingPrograms;
using TrainingService.Dtos.WorkoutPrograms;
using TrainingService.ViewModels.ProgramWorkouts;

namespace TrainingService.ViewModels.TrainingProgram;

public static class TrainingProgramVMMapper{
    public static CreateTrainingProgramDto ToDto(this CreateTrainingProgramVM viewModel){
        return new CreateTrainingProgramDto{
            Title = viewModel.Title,
            Description = viewModel.Description,
            Status = viewModel.Status,
            Goal = viewModel.Goal,
            RequiredLevel = viewModel.RequiredLevel,
            DurationWeeks = viewModel.DurationWeeks,
            DaysPerWeek = viewModel.DaysPerWeek,
            AccessType = viewModel.AccessType
        };
    }

    public static GetTrainingProgramRequestDto ToDto(this GetTrainingProgramRequestVM viewModel){
        return new GetTrainingProgramRequestDto{
            PageIndex = viewModel.PageIndex ?? 1,
            PageSize = viewModel.PageSize ?? 10
        };
    }

    public static GetTrainingProgramVM ToViewModel(this GetTrainingProgramDto dto){
        return new GetTrainingProgramVM{
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Goal = dto.Goal,
            RequiredLevel = dto.RequiredLevel,
            DurationWeeks = dto.DurationWeeks,
            DaysPerWeek = dto.DaysPerWeek,
            AccessType = dto.AccessType
        };
    }

    public static AssignWorkoutToProgramDto ToDto(this AssignWorkoutToProgramVM viewModel,Guid programId) {

        return new AssignWorkoutToProgramDto {
            TrainingProgramId = programId,
            WorkoutId = viewModel.WorkoutId,
            WeekNumber = viewModel.WeekNumber,
            DayNumber = viewModel.DayNumber
        };
    }
}