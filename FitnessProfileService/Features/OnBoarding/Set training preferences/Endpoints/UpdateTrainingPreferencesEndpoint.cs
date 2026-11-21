using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Orchestrators;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.ViewModels;
using FitnessProfileService.Features.Shared;
using MediatR;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Endpoints
{
    public static class UpdateTrainingPreferencesEndpoint
    {
        public static void MapUpdateTrainingPreferencesEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPut("api/fitness-profile/training-preferences", async (
                UpdateTrainingPreferencesViewModel viewModel,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                // Temporary UserId until Identity integration.
                var userId = Guid.Parse(
                    "11111111-1111-1111-1111-111111111111");

                var orchestrator = new UpdateTrainingPreferencesOrchestrator(
                    userId,
                    viewModel.TrainingExperienceLevel,
                    viewModel.AvailableDaysPerWeek
                );

                await mediator.Send(orchestrator, cancellationToken);

                return Results.Ok(
                    ApiResponse.Ok(
                        null,
                        "Training preferences updated successfully",
                        StatusCodes.Status200OK));
            })
            .WithTags("Training Preferences")
            .WithSummary("Update training preferences");
        }
    }
}