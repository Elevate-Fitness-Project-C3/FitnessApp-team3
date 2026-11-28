namespace NutritionService.Events
{
    public sealed record UserTdeeCalculatedEvent(
         Guid UserId,
        decimal Tdee,
        DateTime CalculatedAt);


}
