using TrainingService.Domain.Enums;

namespace TrainingService.Dtos.Exercises
{
    public class GetExercisesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TargetMuscleGroup { get; set; } = string.Empty;
        public int? TrackingTypeID { get; set; }

        public String TrackingType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
