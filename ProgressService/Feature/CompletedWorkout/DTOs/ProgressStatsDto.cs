namespace ProgressService.Feature.CompletedWorkout.DTOs;

public record ProgressStatsDto(
    int TotalCompletedWorkouts,
    DateTimeOffset? LastWorkoutAt);
