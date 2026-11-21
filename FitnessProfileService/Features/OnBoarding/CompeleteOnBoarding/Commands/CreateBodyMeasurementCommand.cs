
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Commands
{
    public record CreateBodyMeasurementCommand(
        Guid UserId,
        decimal HeightCm,
        decimal WeightKg
    ) : IRequest<Unit>;
}
