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
        public async Task<Result<NutritionGoalDto>> Handle(GetMyNutritionPreferenceQuery request, CancellationToken ct)
        {
            var preferenceDto = await _repo.GetAll()
            .AsNoTracking()
            .Where(p => p.UserId == request.UserId)
            .Select(p=>new NutritionGoalDto(
                p.UserId,
                true,
                p.NutritionGoal,
                p.UpdatedAt)).FirstOrDefaultAsync(ct);
            if (preferenceDto is null)
            {
                return Result.Success(new NutritionGoalDto(
                    request.UserId,
                    IsConfigured: false,
                    NutritionGoal: null,
                    UpdatedAt: null));
            }
            return Result.Success(preferenceDto);
        }
    }
}
