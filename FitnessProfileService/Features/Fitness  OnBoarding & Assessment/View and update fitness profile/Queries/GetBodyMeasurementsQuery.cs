using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Dtos;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Queries
{
    public record GetBodyMeasurementsQuery(Guid UserId)
        : IRequest<BodyMeasurementsDto>;
}
