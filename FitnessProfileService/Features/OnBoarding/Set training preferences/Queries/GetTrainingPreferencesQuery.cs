using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Dtos;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Queries
{
    public record GetTrainingPreferencesQuery(
        Guid UserId
    ) : IRequest<TrainingPreferencesDto>;
}
