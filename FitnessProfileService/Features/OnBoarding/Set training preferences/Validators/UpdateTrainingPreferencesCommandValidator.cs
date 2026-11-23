using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Commands;
using FluentValidation;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Validators
{
    public class UpdateTrainingPreferencesCommandValidator
        : AbstractValidator<UpdateTrainingPreferencesCommand>
    {
        public UpdateTrainingPreferencesCommandValidator()
        {
            RuleFor(x => x.TrainingExperienceLevel).NotNull()
                .IsInEnum()
                .WithMessage("Invalid training experience level.");

            RuleFor(x => x.AvailableDaysPerWeek).NotNull()
                .InclusiveBetween(1, 7)
                .WithMessage("Available days per week must be between 1 and 7.");
        }
    }
}
