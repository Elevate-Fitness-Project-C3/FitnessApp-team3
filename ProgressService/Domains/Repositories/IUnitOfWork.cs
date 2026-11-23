namespace ProgressService.Domains.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
}
