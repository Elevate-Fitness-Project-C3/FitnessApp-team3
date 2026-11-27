using NutritionService.Features.CalorieTarget.Enums;

namespace NutritionService.Features.CalorieTarget.DTOs
{
    public sealed record FitnessProfileTdeeDto(
        FitnessProfileTdeeStatus Status,
        decimal? Tdee,
        DateTime? CalculatedAt = null,
        string? ErrorMessage = null);
}
