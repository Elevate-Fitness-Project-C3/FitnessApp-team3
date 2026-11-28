namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.Dtos
{
    public class BodyMeasurementResponseDto
    {
        public decimal HeightCm { get; set; }

        public decimal WeightKg { get; set; }

        public DateTime RecordedAt { get; set; }
    }
}
