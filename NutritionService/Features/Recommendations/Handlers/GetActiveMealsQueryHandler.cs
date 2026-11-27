using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.Recommendations.DTOs;
using NutritionService.Features.Recommendations.Queries;

namespace NutritionService.Features.Recommendations.Handlers
{
    public class GetActiveMealsQueryHandler : IRequestHandler<GetActiveMealsQuery, IReadOnlyList<ActiveMealDto>>
    {
        private readonly IGenericRepository<Meal> _mealRepo;

        public GetActiveMealsQueryHandler(IGenericRepository<Meal> mealRepo)
            => _mealRepo = mealRepo;
        public async Task<IReadOnlyList<ActiveMealDto>> Handle(GetActiveMealsQuery request, CancellationToken ct)
        {
            var query = _mealRepo.GetAll()
                .AsNoTracking()
                .Where(m => m.IsActive);
            if (request.MealType.HasValue)
                query = query.Where(m => m.MealType == request.MealType.Value);
            return await query.Select(m => new ActiveMealDto(
                m.MealId,
                m.Name,
                m.MealType,
                m.Calories,
                m.ProteinGrams)).ToListAsync(ct);
        }
    }
}
