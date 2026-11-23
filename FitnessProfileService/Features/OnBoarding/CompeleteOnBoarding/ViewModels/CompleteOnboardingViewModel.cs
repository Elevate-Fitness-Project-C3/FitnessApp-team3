using FitnessProfileService.Common.Enums;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.ViewModels
{
    public class CompleteOnboardingViewModel
    {
        public Gender Gender { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public decimal HeightCm { get; set; }

        public decimal WeightKg { get; set; }

        public PrimaryGoal PrimaryGoal { get; set; }

        public PhysicalActivityLevel PhysicalActivityLevel { get; set; }
    }
}
