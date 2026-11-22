using FluentValidation;
using TrainingService.Features.Exercises.CreateExercise;

namespace TrainingService.Features.Exercises.UpdateExercise
{
    public class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
    {
        public UpdateExerciseCommandValidator()
        {

            RuleFor(x => x.ExerciseId)
                .NotEmpty()
                .WithMessage("Exercise ID is required.");
            RuleFor(x => x.UpdateExerciseDto.Name)
                .NotEmpty()
                .WithMessage("Exercise name is required.")
                .MaximumLength(200);

            RuleFor(x => x.UpdateExerciseDto.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(2000);

            RuleFor(x => x.UpdateExerciseDto.TargetMuscleGroup)
                .NotEmpty()
                .WithMessage("Target muscle group is required.")
                .MaximumLength(100);

            RuleFor(x => x.UpdateExerciseDto.TrackingType)
                .IsInEnum()
                .WithMessage("Invalid exercise tracking type.");
        }
    }
}
