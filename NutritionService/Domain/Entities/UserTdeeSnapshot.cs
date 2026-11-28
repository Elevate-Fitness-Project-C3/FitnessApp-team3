namespace NutritionService.Domain.Entities
{
    public sealed class UserTdeeSnapshot
    {
        public Guid UserId { get; private set; }
        public decimal Tdee { get; private set; }
        public DateTime CalculatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        private UserTdeeSnapshot() { }
        public static UserTdeeSnapshot Create(Guid userId, decimal tdee, DateTime calculatedAt) => new()
        {
            UserId = userId,
            Tdee = tdee,
            CalculatedAt = calculatedAt,
            UpdatedAt = DateTime.UtcNow
        };
        public void Update(decimal tdee, DateTime calculatedAt)
        {
            if (calculatedAt < CalculatedAt)
                return;
            Tdee = tdee;
            CalculatedAt = calculatedAt;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
