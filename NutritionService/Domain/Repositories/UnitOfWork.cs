using Microsoft.EntityFrameworkCore.Storage;
using NutritionService.Comman.Results;
using NutritionService.Domain.Repositories;

namespace NutritionService.Data;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly NutritionDbContext _db;
    private IDbContextTransaction? _tx;
    private int _depth;
    private bool _hasNestedFailure;

    public UnitOfWork(NutritionDbContext db)
        => _db = db;

    public Task<int> SaveChangesAsync(
        CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    public Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken ct = default)
    {
        return ExecuteAsync<object?>(async token =>
        {
            await action(token);
            return null;
        }, ct);
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> action,
        CancellationToken ct = default)
    {
        var isOutermost = _depth == 0;

        if (isOutermost)
        {
            _hasNestedFailure = false;
            _tx = await _db.Database
                .BeginTransactionAsync(ct);
        }

        _depth++;
        try
        {
            var result = await action(ct);

            var isFailure =
                result is Result { IsFailure: true };

            if (isFailure)
                _hasNestedFailure = true;

            if (isOutermost)
            {
                if (_hasNestedFailure)
                {
                    await RollbackAsync(ct);

                    if (isFailure)
                        return result;

                    throw new InvalidOperationException("Commit blocked after nested failure.");
                }

                await _db.SaveChangesAsync(ct);
                await _tx!.CommitAsync(ct);
            }

            return result;
        }
        catch
        {
            _hasNestedFailure = true;
            if (isOutermost)
                await RollbackAsync(ct);
            throw;
        }
        finally
        {
            _depth--;
            if (isOutermost && _tx is not null)
            {
                await _tx.DisposeAsync();
                _tx = null;
            }
        }
    }

    private async Task RollbackAsync(CancellationToken ct)
    {
        if (_tx is not null)
            await _tx.RollbackAsync(ct);
    }
}