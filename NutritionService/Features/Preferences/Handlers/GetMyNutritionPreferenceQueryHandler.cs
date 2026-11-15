using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Comman.Results;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.Preferences.DTOs;
using NutritionService.Features.Preferences.Queries;

namespace NutritionService.Features.Preferences.Handlers
{
    public sealed class GetMyNutritionPreferenceQueryHandler
        :IRequestHandler<GetMyNutritionPreferenceQuery, Result<NutritionGoalDto>>
    {
        private readonly IGenericRepository<NutritionPreference> _repo;

        public GetMyNutritionPreferenceQueryHandler( IGenericRepository<NutritionPreference> repo)
            =>_repo = repo;
        public async Task<Result<NutritionGoalDto>> Handle(GetMyNutritionPreferenceQuery request, CancellationToken cancellationToken)
        {
            var preference = await _repo.GetAll()
            .AsNoTracking().
            FirstOrDefaultAsync(p => p.UserId == request.UserId);
            if (preference is null)

            {
                return Result.Success(new NutritionGoalDto(
                    request.UserId,
                    IsConfigured: false,
                    NutritionGoal: null,
                    UpdatedAt: null));
            }

            return Result.Success(new NutritionGoalDto(
                preference.UserId,
                IsConfigured: true,
                NutritionGoal: preference.NutritionGoal,
                UpdatedAt: preference.UpdatedAt));
        }
    }
}
