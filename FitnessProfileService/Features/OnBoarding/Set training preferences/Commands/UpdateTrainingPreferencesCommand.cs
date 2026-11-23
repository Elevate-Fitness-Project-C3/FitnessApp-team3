using FitnessProfileService.Common.Enums;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Commands
{
    public record UpdateTrainingPreferencesCommand(
            Guid ProfileId,
        Guid UserId,
        TrainingExperienceLevel TrainingExperienceLevel,
        int AvailableDaysPerWeek
    ) : IRequest<Unit>;
}
