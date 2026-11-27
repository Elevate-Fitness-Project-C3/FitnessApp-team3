using MediatR;
using Microsoft.EntityFrameworkCore;
using ProgressService.Domains.Entities;
using ProgressService.Domains.Repositories;
using ProgressService.Feature.CompletedWorkout.DTOs;
using ProgressService.Feature.CompletedWorkout.Queries;

namespace ProgressService.Feature.CompletedWorkout.Handler;

public sealed class GetProgressStatsHandler
    : IRequestHandler<GetProgressStatsQuery, ProgressStatsDto>
{
    private readonly IGenericRepository<CompletedWorkoutRecord> _repository;

    public GetProgressStatsHandler(IGenericRepository<CompletedWorkoutRecord> repository)
    {
        _repository = repository;
    }

    public async Task<ProgressStatsDto> Handle(
        GetProgressStatsQuery query,
        CancellationToken cancellationToken)
    {
        var aggregate = await _repository.GetAll()
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId)
            .GroupBy(x => x.UserId)
            .Select(group => new
            {
                Total = group.Count(),
                Last = group.Max(x => x.CompletedAtUtc)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return aggregate is null
            ? new ProgressStatsDto(0, null)
            : new ProgressStatsDto(aggregate.Total, aggregate.Last);
    }
}
