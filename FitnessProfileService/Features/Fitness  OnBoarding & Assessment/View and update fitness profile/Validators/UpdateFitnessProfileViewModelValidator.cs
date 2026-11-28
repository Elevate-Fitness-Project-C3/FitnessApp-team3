
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.ViewModel;
using FluentValidation;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Validators
{
    public class UpdateFitnessProfileViewModelValidator
        : AbstractValidator<UpdateFitnessProfileViewModel>
    {
        public UpdateFitnessProfileViewModelValidator()
        {
            RuleFor(x => x)
                .Must(HasAtLeastOneValue)
                .WithMessage("At least one field must be provided.");

            RuleFor(x => x.Gender)
                .IsInEnum()
                .When(x => x.Gender.HasValue)
                .WithMessage("Invalid gender value.");

            RuleFor(x => x.DateOfBirth)
                .Must(date => date < DateOnly.FromDateTime(DateTime.UtcNow))
                .When(x => x.DateOfBirth.HasValue)
                .WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.PrimaryGoal)
                .IsInEnum()
                .When(x => x.PrimaryGoal.HasValue)
                .WithMessage("Invalid primary goal.");

            RuleFor(x => x.PhysicalActivityLevel)
                .IsInEnum()
                .When(x => x.PhysicalActivityLevel.HasValue)
                .WithMessage("Invalid physical activity level.");

            RuleFor(x => x.TrainingExperienceLevel)
                .IsInEnum()
                .When(x => x.TrainingExperienceLevel.HasValue)
                .WithMessage("Invalid training experience level.");

            RuleFor(x => x.AvailableDaysPerWeek)
                .InclusiveBetween(1, 7)
                .When(x => x.AvailableDaysPerWeek.HasValue)
                .WithMessage("Available days per week must be between 1 and 7.");

            RuleFor(x => x.HeightCm)
                .InclusiveBetween(50m, 300m)
                .When(x => x.HeightCm.HasValue)
                .WithMessage("Height must be between 50 and 300 cm.");

            RuleFor(x => x.WeightKg)
                .InclusiveBetween(10m, 500m)
                .When(x => x.WeightKg.HasValue)
                .WithMessage("Weight must be between 10 and 500 kg.");
        }

        private static bool HasAtLeastOneValue(UpdateFitnessProfileViewModel model)
        {
            return model.Gender.HasValue
                || model.DateOfBirth.HasValue
                || model.PrimaryGoal.HasValue
                || model.PhysicalActivityLevel.HasValue
                || model.TrainingExperienceLevel.HasValue
                || model.AvailableDaysPerWeek.HasValue
                || model.HeightCm.HasValue
                || model.WeightKg.HasValue;
        }
    }
}
