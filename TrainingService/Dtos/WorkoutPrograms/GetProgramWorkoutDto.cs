namespace TrainingService.Dtos.WorkoutPrograms {
    public class GetProgramWorkoutDto {
        public Guid Id { get; set; }

        public Guid WorkoutId { get; set; }

        public int WeekNumber { get; set; }

        public int DayNumber { get; set; }


    }
}
