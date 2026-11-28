using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Dtos;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Handlers
{
    public class GetBodyMeasurementsQueryHandler(
        IGeneralRepository<BodyMeasurement> repository)
        : IRequestHandler<GetBodyMeasurementsQuery, BodyMeasurementsDto?>
    {
        public async Task<BodyMeasurementsDto?> Handle(
            GetBodyMeasurementsQuery request,
            CancellationToken cancellationToken)
        {
            var bodyMeasurement = await repository.GetAll()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.RecordedAt)
                .Select(x => new BodyMeasurementsDto
                {
                    HeightCm = x.HeightCm,
                    WeightKg = x.WeightKg,
                    RecordedAt = x.RecordedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (bodyMeasurement is null)
                throw new NotFoundException("Body measurements not found.");

            return bodyMeasurement;
        }
    }
}
