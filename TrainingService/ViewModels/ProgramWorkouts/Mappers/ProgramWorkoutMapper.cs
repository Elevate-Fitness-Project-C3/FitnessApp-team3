
using TrainingService.Dtos.WorkoutPrograms;
using TrainingService.ViewModels.ProgramWorkouts;

namespace TrainingService.ViewModels.TrainingProgram;

public static class ProgramWorkoutMapper {
    

    public static AssignWorkoutToProgramDto toProgramDto(this AssignWorkoutToProgramVM viewModel,Guid programId) {


        return new AssignWorkoutToProgramDto {
            TrainingProgramId = programId,
            WorkoutId = viewModel.WorkoutId,
            WeekNumber = viewModel.WeekNumber,
            DayNumber = viewModel.DayNumber
        };
    }


    public static GetProgramWorkoutVM ToVM(this GetProgramWorkoutDto dto) {
        return new GetProgramWorkoutVM {
            Id = dto.Id,
            WorkoutId = dto.WorkoutId,
            WeekNumber = dto.WeekNumber,
            DayNumber = dto.DayNumber
        };
    }

}