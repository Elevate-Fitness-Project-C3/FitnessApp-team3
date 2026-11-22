using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Dtos;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Queries;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Handlers
{
    public class GetTrainingPreferencesQueryHandler(
        IGeneralRepository<FitnessProfile> repository)
        : IRequestHandler<GetTrainingPreferencesQuery, TrainingPreferencesDto>
    {
        public async Task<TrainingPreferencesDto> Handle(
            GetTrainingPreferencesQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await repository.GetAll()
                .FirstOrDefaultAsync(
                    x => x.UserId == request.UserId,
                    cancellationToken);

            if (profile is null)
                throw new NotFoundException("Fitness profile not found.");

            if (!profile.IsOnboardingCompleted)
                throw new BadRequestException(
                    "Complete fitness onboarding before updating training preferences.");

            return profile.Adapt<TrainingPreferencesDto>();
        }
    }
}
