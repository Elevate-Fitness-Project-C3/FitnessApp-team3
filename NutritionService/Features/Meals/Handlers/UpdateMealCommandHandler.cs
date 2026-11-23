using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.Meals.Commands;

namespace NutritionService.Features.Meals.Handlers
{
    public sealed class UpdateMealCommandHandler : IRequestHandler<UpdateMealCommand, Result>
    {
        private readonly IGenericRepository<Meal> _repo;
        private readonly IUnitOfWork _uow;
        public UpdateMealCommandHandler(IGenericRepository<Meal> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }
        public async Task<Result> Handle(UpdateMealCommand request, CancellationToken ct)
        {
            var meal = await _repo.GetByIdAsync(request.MealId, ct);
            if (meal is null)
                return Result.Failure(Error.NotFound(
                    "Meal.Notfound",
                    "Meal Was NotFound"));
            var updateResult = meal.Update(
                request.Name,
                request.MealType,
                request.Calories,
                request.ProteinGrams);
            if (updateResult.IsFailure)
                return updateResult;
            _repo.SaveInclude(
                meal,
                nameof(meal.Name),
                nameof(meal.MealType),
                nameof(meal.Calories),
                nameof(meal.ProteinGrams));
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
