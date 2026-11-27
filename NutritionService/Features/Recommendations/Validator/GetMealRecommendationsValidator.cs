using FluentValidation;
using NutritionService.Features.Recommendations.Orchestrators;
using NutritionService.Features.Recommendations.Queries;

namespace NutritionService.Features.Recommendations.Validator
{
    public class GetMealRecommendationsValidator : AbstractValidator<GetMealRecommendationsOrchestrator>
    {
        public GetMealRecommendationsValidator()
        {
            RuleFor(x => x.UserId)
             .NotEmpty();

            RuleFor(x => x.MealType)
                .IsInEnum()
                .When(x => x.MealType.HasValue)
                .WithMessage("Invalid mealType value.");
        }
    }
}
