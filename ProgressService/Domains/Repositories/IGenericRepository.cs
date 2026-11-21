namespace ProgressService.Domains.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetAll();
    void Add(T entity);
}
