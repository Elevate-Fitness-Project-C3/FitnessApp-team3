namespace NutritionService.Domain.Repositories
{
    public interface IGenericRepository <T> where T : class
    {
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveInclude(T entity, params string[] properties);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
