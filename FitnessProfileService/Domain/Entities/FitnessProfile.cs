using FitnessProfileService.Common.Enums;
using System.Reflection;

namespace FitnessProfileService.Domain.Entities
{
    public class FitnessProfile
    {
        public Guid UserId { get; set; }

        public Gender Gender { get; set; }

        public DateOnly DateOfBirth { get; set; }
        public bool IsOnboardingCompleted { get; set; }

        public PrimaryGoal PrimaryGoal { get; set; }

        public PhysicalActivityLevel PhysicalActivityLevel { get; set; }

        public TrainingExperienceLevel TrainingExperienceLevel { get; set; }

        public int AvailableDaysPerWeek { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<BodyMeasurement> BodyMeasurements { get; set; }
            = new List<BodyMeasurement>();
    }
}
