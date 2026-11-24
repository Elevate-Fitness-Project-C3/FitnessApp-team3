using System.Linq.Expressions;
using TrainingService.Domain.Common;

namespace TrainingService.Infrastructure.DataAccess;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
    IQueryable<T> GetAll();
    IQueryable<T> Get(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    Task UpdateAsync(T entity);
    void Delete(T entity);
    Task DeleteAsync(T entity);
    void HardDelete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    Task<int> CountAsync(Expression<Func<T, bool>>? criteria = null);
    void SaveInclude( T entity,params string[] includedProperties);
}
