using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Commands;
using Mapster;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Handlers
{
    public class CreateBodyMeasurementCommandHandler(
        IGeneralRepository<BodyMeasurement> repository)
        : IRequestHandler<CreateBodyMeasurementCommand, Unit>
    {
        public Task<Unit> Handle(
            CreateBodyMeasurementCommand request,
            CancellationToken cancellationToken)
        {
            var measurement = new BodyMeasurement()
            {
                HeightCm = request.HeightCm,
                WeightKg = request.WeightKg,
                UserId = request.UserId

            };

            measurement.Id = Guid.NewGuid();
            measurement.RecordedAt = DateTime.UtcNow;

            repository.Add(measurement);

            return Task.FromResult(Unit.Value);
        }
    }
}
