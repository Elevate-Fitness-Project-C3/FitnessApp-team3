using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Commands;
using Mapster;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Handlers
{
    public class CreateFitnessProfileCommandHandler(
        IGeneralRepository<FitnessProfile> repository)
        : IRequestHandler<CreateFitnessProfileCommand, Unit>
    {
        public Task<Unit> Handle(
            CreateFitnessProfileCommand request,
            CancellationToken cancellationToken)
        {
            var profile = new FitnessProfile()
            {
                UserId = request.UserId,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                PrimaryGoal = request.PrimaryGoal,
                PhysicalActivityLevel = request.PhysicalActivityLevel
            };  

            profile.CreatedAt = DateTime.UtcNow;
            profile.IsOnboardingCompleted = false;

            repository.Add(profile);

            return Task.FromResult(Unit.Value);
        }
    }
}
