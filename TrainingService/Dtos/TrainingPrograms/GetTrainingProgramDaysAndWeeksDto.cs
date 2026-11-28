using TrainingService.Domain.Enums;

namespace TrainingService.Dtos.TrainingPrograms {
    public class GetTrainingProgramDaysAndWeeksDto {

        public Guid Id { get; set; }

        public TrainingProgramStatus Status { get; set; }


        public int DurationWeeks { get; set; }
        public int DaysPerWeek { get; set; }

    }
}
