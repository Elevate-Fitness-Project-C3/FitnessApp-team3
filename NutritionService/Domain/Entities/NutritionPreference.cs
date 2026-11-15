using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;

namespace NutritionService.Domain.Entities
{
    public sealed class NutritionPreference
    {
        private NutritionPreference() { }

        public Guid UserId { get; private set; }
        public NutritionGoal NutritionGoal { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public static Result<NutritionPreference> Create(Guid userId, NutritionGoal nutritionGoal)
        {
            if (userId == Guid.Empty)

                return Result.Failure<NutritionPreference>(Error.Validation(
                    "NutritionPreference.InvalidUserId",
                    "User ID cannot be empty."));

            if (!Enum.IsDefined<NutritionGoal>(nutritionGoal))
                return Result.Failure<NutritionPreference>(Error.Validation(
                    "NutritionPreference.InvalidGoal",
                    "Nutrition goal must be Maintain, GradualLoss, or GradualGain."));
            return Result.Success(new NutritionPreference
            {
                UserId = userId,
                NutritionGoal = nutritionGoal,
                UpdatedAt = DateTime.UtcNow
            });
        }
        public Result UpdateGoal(NutritionGoal newGoal)
        {
            if (!Enum.IsDefined<NutritionGoal>(newGoal))

                return Result.Failure(Error.Validation(
                    "NutritionPreference.InvalidGoal",
                    "Nutrition goal must be Maintain, GradualLoss, or GradualGain."));
            NutritionGoal = newGoal;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
