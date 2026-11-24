using TrainingService.Domain.Common;
using TrainingService.Domain.Enums;

namespace TrainingService.Domain.Entities
{
    public class Workout : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public TrainingLevel Difficulty { get; set; }
        public int EstimatedDurationMinutes { get; set; }

        public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
            = new List<WorkoutExercise>();

        public ICollection<ProgramWorkout> ProgramWorkouts { get; set; }
            = new List<ProgramWorkout>();
    }

}
