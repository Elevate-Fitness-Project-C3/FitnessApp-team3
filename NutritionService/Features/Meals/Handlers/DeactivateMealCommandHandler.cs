using MediatR;
using NutritionService.Comman.Results;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.Meals.Commands;

namespace NutritionService.Features.Meals.Handlers
{
    public record class DeactivateMealCommandHandler : IRequestHandler<DeactivateMealCommand, Result>
    {
        private readonly IGenericRepository<Meal> _repo;
        private readonly IUnitOfWork _uow;

        public DeactivateMealCommandHandler(IGenericRepository<Meal> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }
        public async Task<Result> Handle(DeactivateMealCommand request, CancellationToken ct)
        {
            var meal = await _repo.GetByIdAsync(request.MealId, ct);
            if (meal is null)
                return Result.Failure(Error.NotFound("Meal.NotFound","Meal Was NotFound"));
            meal.Deactivate();   // false Here 
            _repo.SaveInclude(meal, nameof(meal.IsActive));  
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}   
