using MediatR;
using ProgressService.Feature.CompletedWorkout.DTOs;

namespace ProgressService.Feature.CompletedWorkout.Queries;

public record GetProgressStatsQuery(Guid UserId) : IRequest<ProgressStatsDto>;
