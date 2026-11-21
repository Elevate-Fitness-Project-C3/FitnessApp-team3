using MediatR;
using NutritionService.Features.CalorieTarget.DTOs;

namespace NutritionService.Features.CalorieTarget.Queries
{
    public sealed record GetUserTdeeQuery(Guid UserId) : IRequest<FitnessProfileTdeeDto>;
    
}
