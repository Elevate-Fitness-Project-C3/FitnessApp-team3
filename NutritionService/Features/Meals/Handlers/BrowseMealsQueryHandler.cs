using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Comman.Results;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.Meals.DTOs;
using NutritionService.Features.Meals.Queries;

namespace NutritionService.Features.Meals.Handlers
{
    public sealed class BrowseMealsQueryHandler : IRequestHandler<BrowseMealsQuery, Result<List<MealDto>>>
    {
        private readonly IGenericRepository<Meal> _repo;
        public BrowseMealsQueryHandler(IGenericRepository<Meal> repo)
            => _repo = repo;
        public async Task<Result<List<MealDto>>> Handle(BrowseMealsQuery request, CancellationToken ct)
        {
            var query = _repo.GetAll()
                             .AsNoTracking()
                             .Where(m => m.IsActive);
            if (!string.IsNullOrWhiteSpace(request.Search))
                query.Where(m => m.Name.Contains(request.Search.Trim()));

            if (request.MealType.HasValue)
                query = query.Where(m => m.MealType == request.MealType.Value);

            var meals = await query
                .OrderBy(m => m.Name)
                .Select(m => new MealDto(
                    m.MealId,
                    m.Name,
                    m.MealType,
                    m.Calories,
                    "kcal",
                    m.ProteinGrams,
                    "g",
                    m.IsActive))
                .ToListAsync(ct);
            return Result.Success(meals);
        }
    }
}
