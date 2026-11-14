namespace IdentityService.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
    Task AddSavePointAsync(string name, CancellationToken cancellationToken = default);
    Task RollbackToSavePointAsync(string name, CancellationToken cancellationToken = default);
}
