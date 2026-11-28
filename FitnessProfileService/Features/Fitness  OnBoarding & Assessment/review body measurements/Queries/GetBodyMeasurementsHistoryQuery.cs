using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.Dtos;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.Queries
{
    public record GetBodyMeasurementsHistoryQuery(Guid UserId)
        : IRequest<IReadOnlyList<BodyMeasurementResponseDto>>;
}
