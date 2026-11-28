using FitnessProfileService.Common.Enums;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.ViewModel
{
    public class UpdateFitnessProfileViewModel
    {
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }

        public PrimaryGoal? PrimaryGoal { get; set; }
        public PhysicalActivityLevel? PhysicalActivityLevel { get; set; }

        public TrainingExperienceLevel? TrainingExperienceLevel { get; set; }
        public int? AvailableDaysPerWeek { get; set; }

        public decimal? HeightCm { get; set; }
        public decimal? WeightKg { get; set; }
    }
}
