using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Commands;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Orchestrators;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Handlers
{
    public class UpdateFitnessProfileOrchestratorHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateFitnessProfileOrchestrator, Unit>
    {
        public async Task<Unit> Handle(
            UpdateFitnessProfileOrchestrator request,
            CancellationToken cancellationToken)
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                var hasProfileChanges =
                    request.ViewModel.Gender.HasValue ||
                    request.ViewModel.DateOfBirth.HasValue ||
                    request.ViewModel.PrimaryGoal.HasValue ||
                    request.ViewModel.PhysicalActivityLevel.HasValue ||
                    request.ViewModel.TrainingExperienceLevel.HasValue ||
                    request.ViewModel.AvailableDaysPerWeek.HasValue;
                if (hasProfileChanges)
                {
                    await mediator.Send(
                        new UpdateFitnessProfileCommand(
                            request.UserId,
                            request.ViewModel),
                        cancellationToken);
                }

                await mediator.Send(
                    new UpdateBodyMeasurementCommand(
                        request.UserId,
                        request.ViewModel.HeightCm,
                        request.ViewModel.WeightKg),
                    cancellationToken);

            }, cancellationToken);

            return Unit.Value;
        }
    }
}
