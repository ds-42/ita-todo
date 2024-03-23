using Common.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Persistence;

public class BaseRepository<T> : IRepository<T> where T : class, new()
{
    private readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T[]> GetItemsAsync(
        int? offset = null,
        int? limit = null,
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        bool? destinct = null,
        CancellationToken cancellationToken = default)
    {
        var items = _dbContext.Set<T>().AsQueryable();

        if (predicate != null)
        {
            items = items.Where(predicate);
        }

        if (orderBy != null)
        {
            items = destinct == true
                ? items.OrderBy(orderBy)
                : items.OrderByDescending(orderBy);
        }

        if (offset != null)
        {
            items = items.Skip(offset.Value);
        }

        if (limit != null)
        {
            items = items.Take(limit.Value);
        }

        return await items.ToArrayAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var set = _dbContext.Set<T>();
        return predicate == null
            ? await set.CountAsync(cancellationToken)
            : await set.CountAsync(predicate, cancellationToken);
    }

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        var set = _dbContext.Set<T>();

        return predicate == null
            ? await set.SingleOrDefaultAsync(cancellationToken)
            : await set.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<T> SingleAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        var set = _dbContext.Set<T>();

        return predicate == null
            ? await set.SingleAsync(cancellationToken)
            : await set.SingleAsync(predicate, cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        var set = _dbContext.Set<T>();

        return predicate == null
            ? await set.FirstOrDefaultAsync(cancellationToken)
            : await set.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<T> AddAsync(T item, CancellationToken cancellationToken = default)
    {
        var set = _dbContext.Set<T>();
        set.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<T> UpdateAsync(T item, CancellationToken cancellationToken = default)
    {
        var set = _dbContext.Set<T>();
        set.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<bool> DeleteAsync(T item, CancellationToken cancellationToken = default)
    {
        var set = _dbContext.Set<T>();
        set.Remove(item);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
