namespace FitnessProfileService.Domain.Entities
{
    public class BodyMeasurement
    {
        public Guid MeasurementId { get; set; }

        public Guid UserId { get; set; }

        public decimal HeightCm { get; set; }

        public decimal WeightKg { get; set; }

        public DateTime RecordedAt { get; set; }

        public FitnessProfile FitnessProfile { get; set; } = null!;
    }
}
