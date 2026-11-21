using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Commands;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Orchestrators;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Queries;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Handlers
{
    public class UpdateTrainingPreferencesOrchestratorHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateTrainingPreferencesOrchestrator, Unit>
    {
        public async Task<Unit> Handle(
            UpdateTrainingPreferencesOrchestrator request,
            CancellationToken cancellationToken)
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
  
                var profile = await mediator.Send(
                    new GetTrainingPreferencesQuery(request.UserId),
                    cancellationToken);


                await mediator.Send(
                    new UpdateTrainingPreferencesCommand(
                        profile.Id,
                        profile.UserId,
                        request.TrainingExperienceLevel,
                        request.AvailableDaysPerWeek),
                    cancellationToken);

            }, cancellationToken);

            return Unit.Value;
        }
    }
}
