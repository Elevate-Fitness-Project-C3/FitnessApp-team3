using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Handlers;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Orchestrators
{
    public record GetFitnessProfileOrchestrator(Guid UserId)
        : IRequest<GetFitnessProfileResponseDto>;

}
