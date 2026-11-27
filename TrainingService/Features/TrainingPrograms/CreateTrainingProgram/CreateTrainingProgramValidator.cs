using FluentValidation;

namespace TrainingService.Features.TrainingPrograms.CreateTrainingProgram {
    public class CreateTrainingProgramValidator :AbstractValidator<GetTrainingProgramCommand> {
        public CreateTrainingProgramValidator()
        {
            RuleFor(x => x.trainingProgramDto.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.trainingProgramDto.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.trainingProgramDto.Goal)
           .IsInEnum();

            RuleFor(x => x.trainingProgramDto.RequiredLevel)
                .IsInEnum();

            RuleFor(x => x.trainingProgramDto.DurationWeeks)
                .GreaterThan(0).WithMessage("Duration Weeks must be greater than 0.");

            RuleFor(x => x.trainingProgramDto.DaysPerWeek)
           .InclusiveBetween(1, 7).WithMessage("Days per Week must be between 1 and 7.");

            RuleFor(x => x.trainingProgramDto.AccessType)
            .IsInEnum();
        }
    }
}