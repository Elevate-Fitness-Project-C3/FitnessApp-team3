namespace NutritionService.Options
{
    public sealed class CalorieTargetOptions
    {
        public const string SectionName = "CalorieTarget";
        public decimal GradualLossDeficit { get; set; } = 400m;
        public decimal GradualGainSurplus { get; set; } = 300m;
        public decimal MinimumDailyCalories { get; set; } = 1200m;
        public int MaxTdeeAgeDays { get; set; } = 30;
        public string Disclaimer { get; set; } =
            "This daily calorie target is an informational estimate only and does not constitute a medical prescription.";
    }

}
