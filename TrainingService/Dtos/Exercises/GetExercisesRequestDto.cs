namespace TrainingService.Dtos.Exercises
{
    public class GetExercisesRequestDto
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
