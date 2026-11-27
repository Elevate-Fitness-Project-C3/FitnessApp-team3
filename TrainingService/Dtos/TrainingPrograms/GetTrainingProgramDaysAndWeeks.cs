namespace TrainingService.Dtos.TrainingPrograms {
    public class GetTrainingProgramDaysAndWeeks {

        public Guid Id { get; set; }

        public int DurationWeeks { get; set; }
        public int DaysPerWeek { get; set; }

    }
}
