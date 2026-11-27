using FluentValidation;

namespace TrainingService.Features.Exercises.CreateExercise;

public class CreateExerciseCommandValidator
    : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
        RuleFor(x => x.CreateExerciseDto.Name)
            .NotEmpty()
            .WithMessage("Exercise name is required.")
            .MaximumLength(200);

        RuleFor(x => x.CreateExerciseDto.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(2000);

        RuleFor(x => x.CreateExerciseDto.TargetMuscleGroup)
            .NotEmpty()
            .WithMessage("Target muscle group is required.")
            .MaximumLength(100);

        RuleFor(x => x.CreateExerciseDto.TrackingType)
            .IsInEnum()
            .WithMessage("Invalid exercise tracking type.");
    }
}
