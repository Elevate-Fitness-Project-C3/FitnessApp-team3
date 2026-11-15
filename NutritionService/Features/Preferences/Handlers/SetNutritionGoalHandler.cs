using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.Preferences.Commands;

namespace NutritionService.Features.Preferences.Handlers
{
    public sealed class SetNutritionGoalHandler : IRequestHandler<SetNutritionGoalCommand, Result>
    {
        private readonly IGenericRepository<NutritionPreference> _repo;
        public SetNutritionGoalHandler(IGenericRepository<NutritionPreference> repo)
            =>_repo = repo;
        public async Task<Result> Handle(SetNutritionGoalCommand request, CancellationToken ct)
        {
            var preference = await _repo.GetByIdAsync(request.UserID, ct);
            if (preference is null)
            {
                var createResult = NutritionPreference.Create(request.UserID, request.NutritionGoal);
                if (createResult.IsFailure)
                    return Result.Failure(createResult.Error);
                _repo.Add(createResult.Value);
            }
            else
            {
                var updateResult = preference.UpdateGoal(request.NutritionGoal);
                if (updateResult.IsFailure)
                    return updateResult;
            }
            await _repo.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
