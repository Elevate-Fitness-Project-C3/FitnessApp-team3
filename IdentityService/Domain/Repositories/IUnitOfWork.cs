using System.Data;
using IdentityService.Domain.Common;

namespace IdentityService.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    bool CommitMayHaveSucceeded { get; }
    IGeneralRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default,
        IsolationLevel isolationLevel = IsolationLevel.Unspecified);
    Task AddSavePointAsync(string name, CancellationToken cancellationToken = default);
    Task RollbackToSavePointAsync(string name, CancellationToken cancellationToken = default);
}
