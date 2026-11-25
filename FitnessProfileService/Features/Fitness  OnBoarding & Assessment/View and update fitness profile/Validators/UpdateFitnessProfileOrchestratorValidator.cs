using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Orchestrators;
using FluentValidation;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Validators
{
    public class UpdateFitnessProfileOrchestratorValidator
        : AbstractValidator<UpdateFitnessProfileOrchestrator>
    {
        public UpdateFitnessProfileOrchestratorValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.ViewModel)
                .NotNull()
                .WithMessage("Update data is required.")
                .SetValidator(new UpdateFitnessProfileViewModelValidator());
        }
    }
}
