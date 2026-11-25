using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.ViewModel;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Commands
{
    public record UpdateFitnessProfileCommand(
        Guid UserId,
        UpdateFitnessProfileViewModel ViewModel
    ) : IRequest<Unit>;
}
