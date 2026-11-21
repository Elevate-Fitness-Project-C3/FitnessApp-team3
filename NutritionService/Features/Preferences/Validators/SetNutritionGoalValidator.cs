using FluentValidation;
using NutritionService.Features.Preferences.Commands;

namespace NutritionService.Features.Preferences.Validators
{
    public sealed class SetNutritionGoalValidator : AbstractValidator<SetNutritionGoalCommand>
    {
        public SetNutritionGoalValidator()
        {
            RuleFor(x => x.UserID)
           .NotEmpty()
           .WithMessage("UserID Is Required");
            RuleFor(x => x.NutritionGoal)
                .IsInEnum()
                .WithMessage("Goal must be Maintain, GradualLoss, or GradualGain.");
        }
    }
}
