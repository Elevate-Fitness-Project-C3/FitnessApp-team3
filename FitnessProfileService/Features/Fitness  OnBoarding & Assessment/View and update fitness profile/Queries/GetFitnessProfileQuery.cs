using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Dtos;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Queries
{
    public record GetFitnessProfileQuery(Guid UserId)
        : IRequest<FitnessProfileDto>;
}
