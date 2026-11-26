using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Orchestrators;
using FluentValidation;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.CompeleteOnBoarding.Validators
{
    public class CompleteOnboardingOrchestratorValidator
        : AbstractValidator<CompleteOnboardingOrchestrator>
    {
        public CompleteOnboardingOrchestratorValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage("Invalid gender value.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.HeightCm)
                .InclusiveBetween(50m, 300m)
                .WithMessage("Height must be between 50 and 300 cm.");

            RuleFor(x => x.WeightKg)
                .InclusiveBetween(10m, 500m)
                .WithMessage("Weight must be between 10 and 500 kg.");

            RuleFor(x => x.PrimaryGoal)
                .IsInEnum()
                .WithMessage("Invalid primary goal value.");

            RuleFor(x => x.PhysicalActivityLevel)
                .IsInEnum()
                .WithMessage("Invalid physical activity level value.");
        }
    }
}
