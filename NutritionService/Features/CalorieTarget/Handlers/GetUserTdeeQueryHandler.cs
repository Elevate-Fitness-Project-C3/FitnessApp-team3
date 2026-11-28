using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Repositories;
using NutritionService.Features.CalorieTarget.DTOs;
using NutritionService.Features.CalorieTarget.Enums;
using NutritionService.Features.CalorieTarget.Queries;

namespace NutritionService.Features.CalorieTarget.Handlers
{
    public class GetUserTdeeQueryHandler : IRequestHandler<GetUserTdeeQuery, FitnessProfileTdeeDto>
    {
        private readonly IGenericRepository<UserTdeeSnapshot> _repo;
        private readonly ILogger<GetUserTdeeQueryHandler> _logger;

        public GetUserTdeeQueryHandler(
            IGenericRepository<UserTdeeSnapshot> repo,
            ILogger<GetUserTdeeQueryHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<FitnessProfileTdeeDto> Handle(GetUserTdeeQuery request, CancellationToken ct)
        {
            if (request.UserId == Guid.Empty)
            {
                return new FitnessProfileTdeeDto(
                    FitnessProfileTdeeStatus.ProfileIncomplete,
                    Tdee: null,
                    ErrorMessage: "Invalid user identifier.");
            }

            var snapshot = await _repo.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, ct);

            if (snapshot is null || snapshot.Tdee <= 0)
            {
                _logger.LogInformation(
                    "No valid TDEE snapshot found for UserId {UserId}.",
                    request.UserId);

                return new FitnessProfileTdeeDto(
                    FitnessProfileTdeeStatus.ProfileIncomplete,
                    Tdee: null,
                    ErrorMessage: "User fitness profile is incomplete or TDEE has not been calculated yet.");
            }

            return new FitnessProfileTdeeDto(
                FitnessProfileTdeeStatus.Available,
                Tdee: snapshot.Tdee,
                CalculatedAt: snapshot.CalculatedAt);
        }
    }
}