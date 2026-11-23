using FluentValidation;
using ProgressService.Feature.CompletedWorkout.Queries;

namespace ProgressService.Feature.CompletedWorkout.Validator;

public sealed class GetWorkoutHistoryValidator : AbstractValidator<GetWorkoutHistoryQuery>
{
    public GetWorkoutHistoryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
