using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.Dtos;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.Queries;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.Handlers
{
    public class GetLatestBodyMeasurementQueryHandler(
        IGeneralRepository<BodyMeasurement> repository)
        : IRequestHandler<GetLatestBodyMeasurementQuery, BodyMeasurementResponseDto?>
    {
        public async Task<BodyMeasurementResponseDto?> Handle(
            GetLatestBodyMeasurementQuery request,
            CancellationToken cancellationToken)
        {
            return await repository.GetAll()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.RecordedAt)
                .ThenByDescending(x => x.Id)
                .Select(x => new BodyMeasurementResponseDto
                {
                    HeightCm = x.HeightCm,
                    WeightKg = x.WeightKg,
                    RecordedAt = x.RecordedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
