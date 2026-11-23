using FitnessProfileService.Common.Enums;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Orchestrators
{
    public record UpdateTrainingPreferencesOrchestrator(
        Guid UserId,
        TrainingExperienceLevel TrainingExperienceLevel,
        int AvailableDaysPerWeek
    ) : IRequest<Unit>;
}
