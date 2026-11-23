using MediatR;
using Microsoft.EntityFrameworkCore;
using ProgressService.Domains.Entities;
using ProgressService.Domains.Repositories;
using ProgressService.Feature.CompletedWorkout.DTOs;
using ProgressService.Feature.CompletedWorkout.Queries;
using ProgressService.Feature.Shared;
using ProgressService.Feature.Shared.EventContracts;

namespace ProgressService.Feature.CompletedWorkout.Handler;

public sealed class GetWorkoutHistoryHandler
    : IRequestHandler<GetWorkoutHistoryQuery, PagedResult<WorkoutCompleted>>
{
    private const int MaxPageSize = 100;
    private readonly IGenericRepository<CompletedWorkoutRecord> _repository;

    public GetWorkoutHistoryHandler(IGenericRepository<CompletedWorkoutRecord> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<WorkoutCompleted>> Handle(
        GetWorkoutHistoryQuery query,
        CancellationToken cancellationToken)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var baseQuery = _repository.GetAll()
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderByDescending(x => x.CompletedAtUtc)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new WorkoutCompleted(
                x.UserId,
                x.SessionId,
                x.ProgramId,
                x.ProgramWorkoutId,
                x.CompletedAtUtc))
            .ToListAsync(cancellationToken);

        return new PagedResult<WorkoutCompleted>(items, page, pageSize, totalCount);
    }
}
