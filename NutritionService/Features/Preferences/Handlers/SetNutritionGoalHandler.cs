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
        private readonly IUnitOfWork _uow;

        public SetNutritionGoalHandler(IGenericRepository<NutritionPreference> repo,IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Task<Result> Handle(SetNutritionGoalCommand request, CancellationToken ct)
        {
            return _uow.ExecuteAsync(async token =>
            {

                var preference = await _repo.GetByIdAsync(request.UserID,token);
                if (preference is null)
                {
                    var createResult = NutritionPreference.Create(request.UserID, request.NutritionGoal);
                    if (createResult.IsFailure)
                        return Result.Failure(createResult.Error);

                    _repo.Add(createResult.Value);
                    return Result.Success();
                }
                return preference.UpdateGoal(request.NutritionGoal);
            }, ct);
        }
    }
}
