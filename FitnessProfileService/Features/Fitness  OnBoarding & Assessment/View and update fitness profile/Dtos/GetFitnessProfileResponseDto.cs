using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Dtos;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Handlers
{
    public record GetFitnessProfileResponseDto(
        FitnessProfileDto FitnessProfile,
        BodyMeasurementsDto? LatestMeasurements);
}
