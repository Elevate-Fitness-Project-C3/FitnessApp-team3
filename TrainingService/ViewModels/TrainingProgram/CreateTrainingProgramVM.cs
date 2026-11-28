using FitnessProfileService.Common.Enums;
using TrainingService.Domain.Enums;

namespace TrainingService.ViewModels.TrainingProgram {
    public class CreateTrainingProgramVM {

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public TrainingProgramStatus Status { get; set; } = TrainingProgramStatus.Draft;

        public FitnessGoal Goal { get; set; }

        public TrainingLevel RequiredLevel { get; set; }

        public int DurationWeeks { get; set; }
        public int DaysPerWeek { get; set; }

        public ProgramAccessType AccessType { get; set; }

    }
}
