using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Commands
{
    public class UpdateBodyMeasurementCommandHandler(
        IGeneralRepository<BodyMeasurement> repository)
        : IRequestHandler<UpdateBodyMeasurementCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateBodyMeasurementCommand request,
            CancellationToken cancellationToken)
        {
            var latestMeasurement = await repository.GetAll()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.RecordedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (latestMeasurement is null)
                throw new NotFoundException("Body measurements not found.");

            var newHeight = request.HeightCm ?? latestMeasurement.HeightCm;
            var newWeight = request.WeightKg ?? latestMeasurement.WeightKg;

            var hasChanges =
                newHeight != latestMeasurement.HeightCm ||
                newWeight != latestMeasurement.WeightKg;

            if (!hasChanges)
                return Unit.Value;

            var measurement = new BodyMeasurement
            {
                UserId = request.UserId,
                HeightCm = newHeight,
                WeightKg = newWeight,
                RecordedAt = DateTime.UtcNow
            };

            repository.Add(measurement);

            return Unit.Value;
        }
    }
}
