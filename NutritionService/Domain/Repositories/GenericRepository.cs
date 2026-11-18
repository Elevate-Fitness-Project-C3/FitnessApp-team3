using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NutritionService.Data;

namespace NutritionService.Domain.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly NutritionDbContext _context;
        public GenericRepository(NutritionDbContext context)
            => _context = context;
        public void Add(T entity)
        => _context.Set<T>().Add(entity);
        public void Update(T entity)
        => _context.Set<T>().Update(entity);
        public void Delete(T entity)
        => _context.Set<T>().Remove(entity);
        public IQueryable<T> GetAll()
        => _context.Set<T>();
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await _context.Set<T>().ToListAsync(ct);

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Set<T>().FindAsync(new object[] { id }, ct);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
         => await _context.SaveChangesAsync(ct);
        public void SaveInclude(T entity, params string[] properties)
        {
            var keyName = _context.Model
                .FindEntityType(typeof(T))!
                .FindPrimaryKey()!
                .Properties[0].Name;

            var keyProp = typeof(T).GetProperty(keyName)!;
            var entityId = keyProp.GetValue(entity);

            var local = _context.Set<T>().Local
                .FirstOrDefault(e =>
                    keyProp.GetValue(e)!.Equals(entityId));

            EntityEntry entry = local is null
                ? _context.Entry(entity)
                : _context.ChangeTracker.Entries<T>()
                    .First(e => keyProp
                        .GetValue(e.Entity)!
                        .Equals(entityId));

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.IsPrimaryKey())
                    continue;

                prop.IsModified = properties
                    .Contains(prop.Metadata.Name);
            }
        }
    }
}
