namespace TrainingService.Dtos.WorkoutPrograms {
    public class AssignWorkoutToProgramDto {

        public Guid TrainingProgramId { get; set; }

        public Guid WorkoutId { get; set; }

        public int WeekNumber { get; set; }

        public int DayNumber { get; set; }


    }
}
