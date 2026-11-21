using FitnessProfileService.Common.Enums;

using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Commands
{
    public record CreateFitnessProfileCommand(
        Guid UserId,
        Gender Gender,
        DateOnly DateOfBirth,
        PrimaryGoal PrimaryGoal,
        PhysicalActivityLevel PhysicalActivityLevel
    ) : IRequest<Unit>;
}
