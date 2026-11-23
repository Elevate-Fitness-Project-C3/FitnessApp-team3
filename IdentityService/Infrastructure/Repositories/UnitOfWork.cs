using IdentityService.Domain.Common;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IdentityService.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];
    private IDbContextTransaction? _transaction;
    private int _depth;
    private bool _failed;

    public bool CommitMayHaveSucceeded { get; private set; }

    public IGeneralRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        ThrowIfFailed();

        var entityType = typeof(TEntity);
        if (_repositories.TryGetValue(entityType, out var repository))
            return (IGeneralRepository<TEntity>)repository;

        var newRepository = new GeneralRepository<TEntity>(context);
        _repositories.Add(entityType, newRepository);
        return newRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfFailed();
        try
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _failed = true;
            context.ChangeTracker.Clear();
            throw;
        }
    }

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);
        ThrowIfFailed();
        cancellationToken.ThrowIfCancellationRequested();

        var isOuterTransaction = _depth == 0;
        if (isOuterTransaction)
        {
            if (context.Database.CurrentTransaction is not null)
                throw new InvalidOperationException("A transaction is already active outside this unit of work.");

            CommitMayHaveSucceeded = false;
            try
            {
                _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            }
            catch
            {
                _failed = true;
                throw;
            }
        }

        _depth++;
        try
        {
            await action();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfFailed();

            if (isOuterTransaction)
            {
                await SaveChangesAsync(cancellationToken);
                // A lost commit acknowledgment cannot prove that SQL rolled the operation back.
                CommitMayHaveSucceeded = true;
                await _transaction!.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            _failed = true;
            if (isOuterTransaction)
                await RollbackAsync(_transaction!);
            throw;
        }
        finally
        {
            _depth--;
            if (isOuterTransaction)
            {
                try
                {
                    await _transaction!.DisposeAsync();
                }
                catch (Exception exception)
                {
                    if (!_failed)
                    {
                        _failed = true;
                        throw;
                    }

                    // Cleanup must not replace the operation's original failure.
                    logger.LogError("Failed unit-of-work transaction disposal raised {ExceptionType}.",
                        exception.GetType().Name);
                }
                finally
                {
                    _transaction = null;
                }
            }
        }
    }

    public async Task AddSavePointAsync(string name, CancellationToken cancellationToken = default)
    {
        var transaction = GetTransaction(name);
        try
        {
            await SaveChangesAsync(cancellationToken);
            await transaction.CreateSavepointAsync(name, cancellationToken);
        }
        catch
        {
            _failed = true;
            throw;
        }
    }

    public async Task RollbackToSavePointAsync(string name, CancellationToken cancellationToken = default)
    {
        var transaction = GetTransaction(name);
        try
        {
            await transaction.RollbackToSavepointAsync(name, cancellationToken);
            // Reload after rollback so tracked values cannot reapply discarded changes.
            context.ChangeTracker.Clear();
        }
        catch
        {
            _failed = true;
            throw;
        }
    }

    private IDbContextTransaction GetTransaction(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ThrowIfFailed();
        var transaction = _transaction
            ?? throw new InvalidOperationException("Savepoints require an active unit-of-work transaction.");
        if (!transaction.SupportsSavepoints)
            throw new NotSupportedException("The database provider does not support savepoints.");
        return transaction;
    }

    private async Task RollbackAsync(IDbContextTransaction transaction)
    {
        try
        {
            await transaction.RollbackAsync(CancellationToken.None);
        }
        catch (Exception exception)
        {
            logger.LogError("Unit-of-work rollback raised {ExceptionType}; commit may have succeeded {CommitMayHaveSucceeded}.",
                exception.GetType().Name, CommitMayHaveSucceeded);
        }
        finally
        {
            context.ChangeTracker.Clear();
        }
    }

    private void ThrowIfFailed()
    {
        if (_failed)
            throw new InvalidOperationException(
                "This unit of work has failed. End the scope and use a fresh scope for any retry.");
    }

    public void Dispose() => _transaction?.Dispose();
}
