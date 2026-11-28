using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Commands
{
    public record UpdateBodyMeasurementCommand(
        Guid UserId,
        decimal? HeightCm,
        decimal? WeightKg
    ) : IRequest<Unit>;
}
