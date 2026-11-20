namespace NutritionService.Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken ct = default);

    Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken ct = default);
    Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> action,
        CancellationToken ct = default);
}