using TrainingService.Domain.Common;

namespace TrainingService.Domain.Entities
{

    public class ProgramWorkout : BaseEntity
    {
        public Guid TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { get; set; } = null!;

        public Guid WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;

        public int WeekNumber { get; set; }
        public int DayNumber { get; set; }
    }

}
