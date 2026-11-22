using Microsoft.EntityFrameworkCore.Storage;
using TrainingService.Infrastructure.Context;

namespace TrainingService.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly TrainingDbContext _context;
    private IDbContextTransaction? _transaction;
    private int _depth;

    public UnitOfWork(TrainingDbContext context)
    {
        _context = context;
    }

    // Transaction methods
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        bool isOuterTransaction = _depth == 0;

        if (isOuterTransaction)
        {
            _transaction = _context.Database.BeginTransaction();
        }

        _depth++;

        try
        {
            await action();
            if (isOuterTransaction)
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _transaction!.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            if (isOuterTransaction && _transaction != null)
            {
                await _transaction.RollbackAsync(CancellationToken.None);
            }
            throw;
        }
        finally
        {
            _depth--;
            if (isOuterTransaction && _transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }


}
