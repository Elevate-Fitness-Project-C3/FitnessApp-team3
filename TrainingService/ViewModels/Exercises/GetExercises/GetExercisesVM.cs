using TrainingService.Domain.Enums;

namespace TrainingService.ViewModels.Exercises.GetExercises
{
    public class GetExercisesVM
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; } 
        public string? TargetMuscleGroup { get; set; }
        public int? TrackingTypeID { get; set; }
        public String? TrackingType { get; set; } 
        public bool IsActive { get; set; }

    }
}
