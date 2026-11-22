using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProgressService.Domains.Repositories;
using ProgressService.Infrastructure.Persistence;

namespace ProgressService.Infrastructure.Repositories;

public sealed class UnitOfWork(ProgressDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private int _depth;
    private bool _rollbackOnly;

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);
        var isOuterOperation = _depth == 0;

        if (isOuterOperation)
        {
            _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            _rollbackOnly = false;
        }

        _depth++;
        try
        {
            await action();

            if (isOuterOperation)
            {
                if (_rollbackOnly)
                    throw new InvalidOperationException("A nested operation failed; the transaction cannot commit.");

                await context.SaveChangesAsync(cancellationToken);
                await _transaction!.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            _rollbackOnly = true;
            if (isOuterOperation)
            {
                try
                {
                    await _transaction!.RollbackAsync(CancellationToken.None);
                }
                finally
                {
                    context.ChangeTracker.Clear();
                }
            }

            throw;
        }
        finally
        {
            _depth--;
            if (isOuterOperation)
            {
                await _transaction!.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose() => _transaction?.Dispose();
}
