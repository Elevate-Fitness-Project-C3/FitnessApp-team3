using MediatR;
using ProgressService.Feature.CompletedWorkout.DTOs;
using ProgressService.Feature.Shared;
using ProgressService.Feature.Shared.EventContracts;

namespace ProgressService.Feature.CompletedWorkout.Queries;

public record GetWorkoutHistoryQuery(Guid UserId, int Page = 1, int PageSize = 20)
    : IRequest<PagedResult<WorkoutCompleted>>;
