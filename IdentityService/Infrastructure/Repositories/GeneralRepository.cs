using IdentityService.Domain.Common;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

public class GeneralRepository<T>(AppDbContext context) : IGeneralRepository<T> where T : BaseEntity
{
    public IQueryable<T> GetAll() => context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Set<T>().ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Set<T>().FindAsync([id], cancellationToken);

    public void Add(T entity) => context.Set<T>().Add(entity);

    public void Update(T entity) => context.Set<T>().Update(entity);

    public void Delete(T entity) => context.Set<T>().Remove(entity);

    public void SaveInclude(T entity, params string[] properties)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(properties);

        var localEntity = context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);
        var entry = context.Entry(localEntity ?? entity);
        var selectedProperties = properties.Select(name =>
        {
            var property = entry.Metadata.FindProperty(name)
                ?? throw new ArgumentException($"Unknown scalar property '{name}'.", nameof(properties));
            if (property.IsPrimaryKey())
                throw new ArgumentException("Primary keys cannot be updated.", nameof(properties));
            return (Name: name, Value: context.Entry(entity).Property(name).CurrentValue);
        }).ToArray();

        if (!entry.IsKeySet || entry.State == EntityState.Added || entry.State == EntityState.Deleted)
            throw new InvalidOperationException("Partial updates require an existing entity.");

        if (entry.State == EntityState.Detached)
            context.Set<T>().Attach(entry.Entity);

        foreach (var property in entry.Properties)
            property.IsModified = false;

        foreach (var property in selectedProperties)
        {
            var propertyEntry = entry.Property(property.Name);
            propertyEntry.CurrentValue = property.Value;
            propertyEntry.IsModified = true;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
