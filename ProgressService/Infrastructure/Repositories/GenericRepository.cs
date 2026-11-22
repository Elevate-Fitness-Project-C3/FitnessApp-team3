using ProgressService.Domains.Repositories;
using ProgressService.Infrastructure.Persistence;

namespace ProgressService.Infrastructure.Repositories;

public sealed class GenericRepository<T>(ProgressDbContext context) : IGenericRepository<T>
    where T : class
{
    public IQueryable<T> GetAll() => context.Set<T>();

    public void Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        context.Set<T>().Add(entity);
    }
}
