
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
            var preferences = await repository.GetAll()
                .Where(x => x.UserId == request.UserId)
                .Select(x => new TrainingPreferencesDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    IsOnboardingCompleted = x.IsOnboardingCompleted,
                    TrainingExperienceLevel = x.TrainingExperienceLevel,
                    AvailableDaysPerWeek = x.AvailableDaysPerWeek
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (preferences is null)
                throw new NotFoundException("Fitness profile not found.");

            if (!preferences.IsOnboardingCompleted)
                throw new BadRequestException(
                    "Complete fitness onboarding before updating training preferences.");

            return preferences;
        }

    }
}
