
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Commands
{
    public record CompleteOnboardingCommand(
        Guid UserId
    ) : IRequest<Unit>;
}
