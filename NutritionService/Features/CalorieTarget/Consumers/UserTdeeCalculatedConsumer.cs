using MassTransit;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Events;

namespace NutritionService.Features.CalorieTarget.Consumers
{
    public class UserTdeeCalculatedConsumer : IConsumer<UserTdeeCalculatedEvent>
    {
        private readonly IGenericRepository<UserTdeeSnapshot> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UserTdeeCalculatedConsumer> _logger;

        public UserTdeeCalculatedConsumer(
            IGenericRepository<UserTdeeSnapshot> repo,
            IUnitOfWork uow,
            ILogger<UserTdeeCalculatedConsumer> logger)
        {
            _repo = repo;
            _uow = uow;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserTdeeCalculatedEvent> context)
        {
            var message = context.Message;

            if (message.UserId == Guid.Empty || message.Tdee <= 0)
            {
                _logger.LogWarning(
                    "Ignored invalid UserTdeeCalculatedEvent for UserId {UserId} with TDEE {Tdee}.",
                    message.UserId,
                    message.Tdee);
                return;
            }

            var existingSnapshot = await _repo.GetByIdAsync(message.UserId, context.CancellationToken);

            if (existingSnapshot is null)
            {
                var snapshot = UserTdeeSnapshot.Create(
                    message.UserId,
                    message.Tdee,
                    message.CalculatedAt);

                _repo.Add(snapshot);
            }
            else
            {
                existingSnapshot.Update(message.Tdee, message.CalculatedAt);
                _repo.Update(existingSnapshot);
            }

            await _uow.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation(
                "Saved TDEE snapshot ({Tdee}) for UserId {UserId}.",
                message.Tdee,
                message.UserId);
        }
    }
}