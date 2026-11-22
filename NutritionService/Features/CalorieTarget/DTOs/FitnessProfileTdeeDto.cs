using NutritionService.Features.CalorieTarget.Enums;
using NutritionService.Services.FitnessProfile;

namespace NutritionService.Features.CalorieTarget.DTOs
{
    public sealed record FitnessProfileTdeeDto(
        FitnessProfileTdeeStatus Status,
        decimal? Tdee,
        DateTime? CalculatedAt = null,
        string? ErrorMessage = null);
}
