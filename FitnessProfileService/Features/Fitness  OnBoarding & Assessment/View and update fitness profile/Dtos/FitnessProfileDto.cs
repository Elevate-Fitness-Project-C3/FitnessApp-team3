using FitnessProfileService.Common.Enums;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Dtos
{
    public class FitnessProfileDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public PrimaryGoal PrimaryGoal { get; set; }
        public PhysicalActivityLevel PhysicalActivityLevel { get; set; }

        public TrainingExperienceLevel? TrainingExperienceLevel { get; set; }
        public int? AvailableDaysPerWeek { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
