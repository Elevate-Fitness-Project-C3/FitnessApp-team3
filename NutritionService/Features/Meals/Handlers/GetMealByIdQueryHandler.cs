using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Comman.Results;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.Meals.DTOs;
using NutritionService.Features.Meals.Queries;

namespace NutritionService.Features.Meals.Handlers
{
    public class GetMealByIdQueryHandler : IRequestHandler<GetMealByIdQuery, Result<MealDto>>
    {
        private readonly IGenericRepository<Meal> _repo;
        public GetMealByIdQueryHandler(IGenericRepository<Meal> repo)
            => _repo = repo;
        public async Task<Result<MealDto>> Handle(GetMealByIdQuery request, CancellationToken ct)
        {
            var meal = await _repo.GetAll()
                 .AsNoTracking()
                 .Where(m => m.MealId == request.MealId && m.IsActive)
                 .Select(m => new MealDto(
                  m.MealId,
                  m.Name,
                  m.MealType,
                  m.Calories,
                  "kcal",
                  m.ProteinGrams,
                  "g",
                  m.IsActive)).FirstOrDefaultAsync(ct);
            if (meal is null)
                return Result.Failure<MealDto>(
                    Error.NotFound(
                        "Meal.NotFound",
                        "Active meal was not found."));
            return Result.Success(meal);
        }
    }
}
