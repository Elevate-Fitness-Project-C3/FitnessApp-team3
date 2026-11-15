using FluentValidation;
using NutritionService.Features.Preferences.Queries;

namespace NutritionService.Features.Preferences.Validators
{
    public sealed class GetMyNutritionPreferenceValidator : AbstractValidator<GetMyNutritionPreferenceQuery>
    {
        public GetMyNutritionPreferenceValidator()
        => RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId Is Required");
    }
}
