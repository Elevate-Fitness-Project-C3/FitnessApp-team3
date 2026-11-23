using FitnessProfileService.Common.Enums;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Orchestrators
{
    public record CompleteOnboardingOrchestrator(
        Guid UserId,
        Gender Gender,
        DateOnly DateOfBirth,
        decimal HeightCm,
        decimal WeightKg,
        PrimaryGoal PrimaryGoal,
        PhysicalActivityLevel PhysicalActivityLevel
    ) : IRequest<Unit>;
}
