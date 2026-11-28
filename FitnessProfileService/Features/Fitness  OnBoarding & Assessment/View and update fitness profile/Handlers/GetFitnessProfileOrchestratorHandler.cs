using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Orchestrators;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Queries;
using MediatR;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Handlers
{
    public class GetFitnessProfileOrchestratorHandler(IMediator mediator)
        : IRequestHandler<GetFitnessProfileOrchestrator, GetFitnessProfileResponseDto>
    {
        public async Task<GetFitnessProfileResponseDto> Handle(
           GetFitnessProfileOrchestrator request,
           CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.");

            var fitnessProfile = await mediator.Send(
                new GetFitnessProfileQuery(request.UserId),
                cancellationToken);

            var latestMeasurements = await mediator.Send(
                new GetBodyMeasurementsQuery(request.UserId),
                cancellationToken);

            return new GetFitnessProfileResponseDto(
                fitnessProfile,
                latestMeasurements);
        }
    }
}
