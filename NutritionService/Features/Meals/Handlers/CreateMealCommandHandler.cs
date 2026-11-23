using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.Meals.Commands;

namespace NutritionService.Features.Meals.Handlers
{
    public sealed class CreateMealCommandHandler : IRequestHandler<CreateMealCommand, Result<Guid>>
    {
        private readonly IGenericRepository<Meal> _repo;
        private readonly IUnitOfWork _uow;
        public CreateMealCommandHandler(IGenericRepository<Meal> repo , IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<Result<Guid>> Handle(CreateMealCommand request, CancellationToken ct)
        {
            var createResult = Meal.Create(request.Name, request.MealType, request.Calories, request.Calories);
            if (createResult.IsFailure)
                return Result.Failure<Guid>(
                    createResult.Error);
            var meal = createResult.Value;
            _repo.Add(meal);
            await _uow.SaveChangesAsync(ct);
            return Result.Success(meal.MealId);
        }
    }
}
