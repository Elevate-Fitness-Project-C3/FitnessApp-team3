namespace TrainingService.Dtos.Exercises
{
    public class GetTrainingProgramRequestDto
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
