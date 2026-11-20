

using FitnessProfileService.Domain.Common;

namespace FitnessProfileService.Domain.Repositories;

public interface IGeneralRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void SaveInclude(T entity, params string[] properties);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
