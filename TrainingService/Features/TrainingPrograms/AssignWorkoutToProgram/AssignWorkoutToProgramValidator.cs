using FluentValidation;

namespace TrainingService.Features.TrainingPrograms.AssignWorkoutToProgram {
    public class AssignWorkoutToProgramValidator  : AbstractValidator <AssignWorkoutToProgramOrchestrator> {

        public AssignWorkoutToProgramValidator() {

            RuleFor(x => x.RequestDto).NotNull();

            When(x => x.RequestDto != null, () =>
            {
                RuleFor(x => x.RequestDto.TrainingProgramId)
                    .NotEmpty()
                    .WithMessage("Training program ID is required.");

                RuleFor(x => x.RequestDto.WorkoutId)
                    .NotEmpty()
                    .WithMessage("Workout ID is required.");

                RuleFor(x => x.RequestDto.WeekNumber)
                    .GreaterThanOrEqualTo(1);

                RuleFor(x => x.RequestDto.DayNumber)
                    .GreaterThanOrEqualTo(1);
            });
        }
    }
}
