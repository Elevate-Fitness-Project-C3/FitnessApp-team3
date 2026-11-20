using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Commands;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Orchestrators;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Handlers
{
    public class CompleteOnboardingOrchestratorHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CompleteOnboardingOrchestrator, Unit>
    {
        public async Task<Unit> Handle(
            CompleteOnboardingOrchestrator request,
            CancellationToken cancellationToken)
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                await mediator.Send(
                    new CreateFitnessProfileCommand(
                        request.UserId,
                        request.Gender,
                        request.DateOfBirth,
                        request.PrimaryGoal,
                        request.PhysicalActivityLevel),
                    cancellationToken);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await mediator.Send(
                    new CreateBodyMeasurementCommand(
                        request.UserId,
                        request.HeightCm,
                        request.WeightKg),
                    cancellationToken);

                await mediator.Send(
                    new CompleteOnboardingCommand(request.UserId),
                    cancellationToken);

            }, cancellationToken);

            return Unit.Value;
        }
    }
}
