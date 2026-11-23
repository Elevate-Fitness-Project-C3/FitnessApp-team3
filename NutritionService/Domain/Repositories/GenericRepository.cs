using Microsoft.EntityFrameworkCore;
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

        public void SaveInclude(T entity, params string[] properties)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(properties);

            if (properties.Length == 0)
                throw new ArgumentException(
                    "At least one property is required.",
                    nameof(properties));

            var entityType = _context.Model
                .FindEntityType(typeof(T))
                ?? throw new InvalidOperationException(
                    $"Entity '{typeof(T).Name}' not in model.");

            var pk = entityType.FindPrimaryKey()
                ?? throw new InvalidOperationException(
                    $"Entity '{typeof(T).Name}' has no key.");

            var keyValues = pk.Properties
                .Select(p => p.GetGetter().GetClrValue(entity))
                .ToArray();

            var localEntry = _context.ChangeTracker
                .Entries<T>()
                .FirstOrDefault(e => pk.Properties
                    .Select(p => e.Property(p.Name).CurrentValue)
                    .SequenceEqual(keyValues));

            var entry = localEntry ?? _context.Entry(entity);

            if (!entry.IsKeySet
                || entry.State == EntityState.Added
                || entry.State == EntityState.Deleted)
            {
                throw new InvalidOperationException(
                    "Partial updates require an existing entity.");
            }

            var selected = properties
                .Distinct()
                .Select(name =>
                {
                    var prop = entityType.FindProperty(name)
                        ?? throw new ArgumentException(
                            $"Unknown property '{name}'.",
                            nameof(properties));

                    if (prop.IsPrimaryKey())
                        throw new ArgumentException(
                            "Primary keys cannot be updated.",
                            nameof(properties));

                    var val = prop.GetGetter().GetClrValue(entity);
                    return (Name: name, Value: val);
                })
                .ToArray();

            if (entry.State == EntityState.Detached)
                _context.Set<T>().Attach(entry.Entity);

            foreach (var prop in entry.Properties)
                prop.IsModified = false;

            foreach (var item in selected)
            {
                var propEntry = entry.Property(item.Name);
                propEntry.CurrentValue = item.Value;
                propEntry.IsModified = true;
            }
        }
    }
}
