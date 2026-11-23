using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Commands;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Handlers
{
    public class CompleteOnboardingCommandHandler(
        IGeneralRepository<FitnessProfile> repository)
        : IRequestHandler<CompleteOnboardingCommand, Unit>
    {
        public async Task<Unit> Handle(
            CompleteOnboardingCommand request,
            CancellationToken cancellationToken)
        {
            var profile =  repository.GetAll()
                .FirstOrDefault(
                    x => x.UserId == request.UserId);

            if (profile is null)
                throw new KeyNotFoundException("Fitness profile not found.");

            profile.IsOnboardingCompleted = true;

            repository.Update(profile);

            return Unit.Value;
        }
    }
}
