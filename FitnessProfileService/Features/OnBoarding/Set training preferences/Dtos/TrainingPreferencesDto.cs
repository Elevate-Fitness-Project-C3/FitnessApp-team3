using FitnessProfileService.Common.Enums;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Dtos
{
    public class TrainingPreferencesDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsOnboardingCompleted { get; set; }
        public TrainingExperienceLevel? TrainingExperienceLevel { get; set; }
        public int? AvailableDaysPerWeek { get; set; }
    }
}
