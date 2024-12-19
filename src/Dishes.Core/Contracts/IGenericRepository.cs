namespace Dishes.Core.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task InsertAsync(T entity);
    void Update(T entity);
    Task DeleteAsync(T entity);
    void InsertRange(List<T> entities);
    void UpdateRange(List<T> entity);

    IQueryable<T> Table { get; }
    IQueryable<T> TableNoTracking { get; }
}
