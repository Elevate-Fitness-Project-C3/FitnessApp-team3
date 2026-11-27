using FluentValidation;
using TrainingService.Domain.Enums;

namespace TrainingService.Features.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
    {
        public CreateWorkoutCommandValidator()
        {
            RuleFor(x => x.CreateWorkoutDto)
                .NotNull();

            When(x => x.CreateWorkoutDto != null, () =>
            {
                RuleFor(x => x.CreateWorkoutDto.Title)
                    .NotEmpty()
                    .MaximumLength(200);

                RuleFor(x => x.CreateWorkoutDto.Difficulty)
                    .Must(value => Enum.IsDefined(
                        typeof(TrainingLevel), value))
                    .WithMessage("Invalid workout difficulty.");


                RuleFor(x => x.CreateWorkoutDto.Exercises)
                    .NotEmpty()
                    .WithMessage(
                        "Workout must contain at least one exercise.");


                RuleForEach(x => x.CreateWorkoutDto.Exercises)
                    .ChildRules(exercise =>
                    {
                        exercise.RuleFor(x => x.ExerciseId)
                            .NotEmpty();

                        exercise.RuleFor(x => x.Order)
                            .GreaterThan(0);

                        exercise.RuleFor(x => x.Sets)
                            .NotNull()
                            .GreaterThan(0)
                            .WithMessage("Sets must be greater than zero.");

                        exercise.RuleFor(x => x.Reps)
                            .GreaterThan(0)
                            .When(x => x.Reps.HasValue);

                        exercise.RuleFor(x => x.Duration)
                            .GreaterThan(0)
                            .When(x => x.Duration.HasValue);

                        exercise.RuleFor(x => x.Weight)
                            .GreaterThan(0)
                            .When(x => x.Weight.HasValue);

                        exercise.RuleFor(x => x)
                            .Must(x =>
                                x.Reps.HasValue ||
                                x.Duration.HasValue)
                            .WithMessage(
                                "Reps or duration is required.");
                    });

            });
        }
    }
}
