using FitnessProfileService.Common.Enums;
using TrainingService.Domain.Common;
using TrainingService.Domain.Enums;
//using FitnessProfileService.Common.Enums;




namespace TrainingService.Domain.Entities
{

    public class TrainingProgram : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public TrainingProgramStatus Status { get; set; } = TrainingProgramStatus.Draft;

        //Shared with the user profile
        public   FitnessGoal Goal { get; set; } 

        public TrainingLevel RequiredLevel { get; set; }

        public int DurationWeeks { get; set; }
        public int DaysPerWeek { get; set; }

        public ProgramAccessType AccessType { get; set; }
       

        public ICollection<ProgramWorkout> ProgramWorkouts { get; set; } = new List<ProgramWorkout>();
    }

}
