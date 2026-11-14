using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IdentityService.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private int _depth;
    private bool _rollbackOnly;

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);
        var isOuterTransaction = _depth == 0;

        if (isOuterTransaction)
        {
            _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            _rollbackOnly = false;
        }

        _depth++;
        try
        {
            await action();
            if (isOuterTransaction)
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
            if (isOuterTransaction)
            {
                try
                {
                    // Rollback must finish even when the request has been cancelled.
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
            if (isOuterTransaction)
            {
                await _transaction!.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task AddSavePointAsync(string name, CancellationToken cancellationToken = default)
    {
        var transaction = GetTransaction(name);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CreateSavepointAsync(name, cancellationToken);
    }

    public async Task RollbackToSavePointAsync(string name, CancellationToken cancellationToken = default)
    {
        await GetTransaction(name).RollbackToSavepointAsync(name, cancellationToken);
        // Reload entities after rollback so tracked values cannot reapply discarded changes.
        context.ChangeTracker.Clear();
    }

    private IDbContextTransaction GetTransaction(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var transaction = _transaction
            ?? throw new InvalidOperationException("Savepoints require an active unit of work.");
        if (!transaction.SupportsSavepoints)
            throw new NotSupportedException("The database provider does not support savepoints.");
        return transaction;
    }

    public void Dispose() => _transaction?.Dispose();
}
