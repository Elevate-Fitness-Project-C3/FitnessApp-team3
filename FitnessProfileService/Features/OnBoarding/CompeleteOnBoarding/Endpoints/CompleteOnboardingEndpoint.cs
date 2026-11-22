using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Orchestrators;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.ViewModels;
using FitnessProfileService.Features.Shared;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Endpoints
{
    public static class CompleteOnboardingEndpoint
    {
        public static void MapCompleteOnboardingEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("api/fitness-profile/onboarding", async (
                CompleteOnboardingViewModel viewModel,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                // Temporary UserId until Identity integration.
                var userId = Guid.Parse(
                    "11111111-1111-1111-1111-111111111111");

                var orchestrator = new CompleteOnboardingOrchestrator(
                    userId,
                    viewModel.Gender,
                    viewModel.DateOfBirth,
                    viewModel.HeightCm,
                    viewModel.WeightKg,
                    viewModel.PrimaryGoal,
                    viewModel.PhysicalActivityLevel
                );

                await mediator.Send(orchestrator, cancellationToken);

                return Results.Ok(
                    ApiResponse.Ok(
                        null,
                        "Fitness onboarding completed successfully",
                        StatusCodes.Status200OK));
            })
            .WithTags("Fitness Onboarding")
            .WithSummary("Complete fitness onboarding");
        }
    }
}