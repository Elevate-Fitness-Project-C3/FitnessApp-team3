namespace FitnessProfileService.Common.Enums
{
    public static class PhysicalActivityLevelExtensions
    {
        public static decimal GetMultiplier(
            this PhysicalActivityLevel level)
        {
            return level switch
            {
                PhysicalActivityLevel.Sedentary => 1.20m,
                PhysicalActivityLevel.LightlyActive => 1.375m,
                PhysicalActivityLevel.ModeratelyActive => 1.55m,
                PhysicalActivityLevel.VeryActive => 1.725m,
                PhysicalActivityLevel.ExtraActive => 1.90m,

                _ => throw new ArgumentOutOfRangeException(nameof(level))
            };
        }
    }
}
