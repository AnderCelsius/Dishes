using Dishes.Core.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Dishes.Infrastructure.Repositories;

public class GenericRepository<T>(
    DishesDbContext context
) : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public IQueryable<T> Table => _dbSet;

    public IQueryable<T> TableNoTracking => _dbSet.AsNoTracking();

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public void InsertRange(List<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(List<T> entity)
    {
        _dbSet.UpdateRange(entity);
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}

