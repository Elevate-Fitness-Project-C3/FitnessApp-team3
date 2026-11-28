namespace NutritionService.Options
{
    public sealed class MealRecommendationOptions
    {
        public const string SectionName =
        "MealRecommendations";

        public string SingleMealNote { get; set; } =
            "These are individual meal suggestions " +
            "from the active catalog, not a full-day " +
            "meal plan.";
        public decimal BreakfastShare { get; set; } = 0.25m;
        public decimal LunchShare { get; set; } = 0.35m;
        public decimal DinnerShare { get; set; } = 0.30m;
        public decimal SnackShare { get; set; } = 0.10m;
        public decimal DefaultShare { get; set; } = 0.25m;
    }
}
