using NutritionService.Comman.Results;
using NutritionService.Domain.Enums;

namespace NutritionService.Domain.Entities
{
    public sealed class Meal
    {
        private Meal() { }
        public Guid MealId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public MealType MealType { get; private set; }
        public int Calories { get; private set; }
        public decimal ProteinGrams { get; private set; }
        public bool IsActive { get; private set; }

        public static Result<Meal> Create(string name, MealType mealType, int calories, decimal proteinGrams)
        {
            var validation = ValidateInputs(name, mealType, calories, proteinGrams);
            if (validation.IsFailure)
                return Result.Failure<Meal>(validation.Error);
            return Result.Success(new Meal
            {
                MealId = Guid.NewGuid(),
                Name = name.Trim(),
                MealType = mealType,
                Calories = calories,
                ProteinGrams = proteinGrams,
                IsActive = true
            });
        }
        public Result Update(string name, MealType mealType, int calories, decimal proteinGrams)
        {
            var validation = ValidateInputs(name, mealType, calories, proteinGrams);
            if (validation.IsFailure)
                return validation;
            Name = name.Trim();
            MealType = mealType;
            Calories = calories;
            ProteinGrams = proteinGrams;
            return Result.Success();
        }
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
        private static Result ValidateInputs(string name, MealType mealType, int calories, decimal proteinGrams)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(Error.Validation(
                    "Meal.EmptyName",
                    "Meal name is required."));
            if (!Enum.IsDefined<MealType>(mealType))
                return Result.Failure(Error.Validation(
                    "Meal.InvalidMealType",
                    "Meal type must be a valid option."));
            if (calories < 0)
                return Result.Failure(Error.Validation(
                    "Meal.InvalidCalories",
                    "Calories (kcal) must be a positive non-negative value."));
            if (proteinGrams < 0)
                return Result.Failure(Error.Validation(
                    "Meal.InvalidProtein",
                    "Protein (grams) must be non-negative."));
            return Result.Success();
        }
    }
}
