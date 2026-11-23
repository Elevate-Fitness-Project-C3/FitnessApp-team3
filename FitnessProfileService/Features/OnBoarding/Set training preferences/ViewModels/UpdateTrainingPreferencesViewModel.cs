using FitnessProfileService.Common.Enums;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.ViewModels
{
    public record UpdateTrainingPreferencesViewModel(
        TrainingExperienceLevel TrainingExperienceLevel,
        int AvailableDaysPerWeek
    );
}
