using NutritionService.Domain.Enums;
using NutritionService.Features.CalorieTarget.DTOs;
using NutritionService.Features.CalorieTarget.Enums;
using NutritionService.Features.Recommendations.DTOs;
using NutritionService.Options;

namespace NutritionService.Features.Recommendations.Helpers
    {
        public static class MealRecommendationRanker
        {
            private const string SingleMealNote =
            "These are individual meal suggestions " +
            "from the active catalog, not a full-day " +
            "meal plan. A single meal covers only a " +
            "portion of your daily calorie estimate.";
            public static MealRecommendationsDto Build(
                IReadOnlyList<ActiveMealDto> activeMeals,
                DailyCalorieTargetDto? target,
                TargetError? errorCode,
                string? fallbackError,
                MealRecommendationOptions options)
            {
                return target is null
                    ? BuildFallback(activeMeals, errorCode, fallbackError)
                    : BuildPersonalized(activeMeals, target, options);
            }

            private static MealRecommendationsDto BuildPersonalized(
                IReadOnlyList<ActiveMealDto> activeMeals,
                DailyCalorieTargetDto target, MealRecommendationOptions options)
            {
                var dailyTarget = target.DailyCalorieTarget;
                var goal = target.NutritionGoal;
                var items = activeMeals
                    .Select(m =>
                    {
                        var share = GetMealShare(m.MealType, options);
                        // Ideal calories for a single meal
                        var idealCals = Math.Round(dailyTarget * share, 0);
                        // Distance from the ideal portion
                        var diff = Math.Abs(m.Calories - idealCals);
                        //full daily target Percentage 
                        var pct = dailyTarget > 0 ? Math.Round(m.Calories * 100m / dailyTarget, 1) : 0;
                        return new { Meal = m, IdealCals = idealCals, Diff = diff, Pct = pct };

                    })
                    .OrderBy(x => x.Diff)
            .ThenBy(x =>
                goal == NutritionGoal.LoseWeight
                    ? x.Meal.Calories
                    : 0)
            .ThenByDescending(
                x => x.Meal.ProteinGrams)
            .ThenBy(
                x => x.Meal.Name,
                StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.Meal.MealId)
            .Select(x =>
                new MealRecommendationItemDto(
                    x.Meal.MealId,
                    x.Meal.Name,
                    x.Meal.MealType,
                    x.Meal.Calories,
                    x.Meal.ProteinGrams,
                    PercentOfDailyTarget: x.Pct,
                    Reason:
                        $"~{x.Pct:0.#}% of your " +
                        $"{dailyTarget:0} kcal " +
                        "daily target; close to " +
                        $"a {x.Meal.MealType} " +
                        $"portion (~{x.IdealCals:0} " +
                        "kcal) with " +
                        $"{x.Meal.ProteinGrams:0.#}g " +
                        "protein."))
            .ToList();

                return new MealRecommendationsDto(
                    IsPersonalized: true,
                    DailyCalorieTarget: dailyTarget,
                    TargetUnit: target.Unit,
                    NutritionGoal: goal,
                    MissingDataReason: null,
                    Note: SingleMealNote,
                    Meals: items); 
            }
            private static MealRecommendationsDto BuildFallback(
                IReadOnlyList<ActiveMealDto> activeMeals,
                TargetError? errorCode,
                string? fallbackError)
            {

                var items = activeMeals.OrderBy(m => m.MealType)
                                       .ThenBy(m => m.Calories)
                                       .ThenBy(m => m.Name, StringComparer.OrdinalIgnoreCase)
                                       .ThenBy(m => m.MealId)
                                       .Select(m =>
                                        new MealRecommendationItemDto(
                                            m.MealId,
                                            m.Name,
                                            m.MealType,
                                            m.Calories,
                                            m.ProteinGrams,
                                            PercentOfDailyTarget: null,
                                            Reason:
                                                $"Active {m.MealType} " +
                                                "meal sorted by meal " +
                                                "type and calories.")).ToList();

                return new MealRecommendationsDto(
                IsPersonalized: false,
                DailyCalorieTarget: null,
                TargetUnit: null,
                NutritionGoal: null,
                MissingDataReason: MapMissingReason(
                    errorCode,
                    fallbackError),
                Note: SingleMealNote,
                Meals: items);
            }
            private static decimal GetMealShare(
            MealType mealType,
            MealRecommendationOptions options) =>
            mealType switch
            {
                MealType.Breakfast =>
                    options.BreakfastShare,
                MealType.Lunch =>
                    options.LunchShare,
                MealType.Dinner =>
                    options.DinnerShare,
                MealType.Snack =>
                    options.SnackShare,
                _ => options.DefaultShare
            };

            private static string MapMissingReason(
            TargetError? errorCode,
            string? fallbackError) => errorCode switch
            {
                TargetError.GoalNotSet =>
                    "Nutrition goal is not set. Set " +
                    "your goal for personalized ranking.",

                TargetError.ProfileIncomplete =>
                    "Fitness profile is incomplete. " +
                    "Complete it to calculate TDEE.",

                TargetError.TdeeOutdated =>
                    "Your TDEE estimate is outdated. " +
                    "Update your profile to refresh.",

                TargetError.ServiceUnavailable =>
                    "Fitness profile service is " +
                    "unavailable. Showing active catalog.",

                _ => fallbackError
                    ?? "Daily calorie target is " +
                       "currently unavailable."
            };

        }
    }
