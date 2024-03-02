using System.Linq.Expressions;

namespace Common.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class, new()
{
    private static List<T> _items = [];
    // faq: скорее всего объект должен быть статическим

    public T[] GetItems(int? offset = null, int? limit = null, Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>? orderBy = null, bool? destinct = null)
    {
        IEnumerable<T> items = _items;

        if (predicate != null)
        { 
            items = items.Where(predicate.Compile());
        }

        if (orderBy != null)
        {
            items = (destinct ?? false)
                ? items.OrderByDescending(orderBy.Compile())
                : items.OrderBy(orderBy.Compile());
        }

        items = items.Skip(offset ?? 0);

        if (limit != null) 
        {
            items = items.Take(limit.Value); 
        }

        return items.ToArray();
    }

    public int Count(Expression<Func<T, bool>>? predicate = null)
    { 
        return predicate == null
            ? _items.Count()
            : _items.Where(predicate.Compile()).Count();    
    }

    public T? SingleOrDefault(Expression<Func<T, bool>>? predicate) 
    {
        return predicate == null
            ? _items.SingleOrDefault()
            : _items.SingleOrDefault(predicate.Compile());
    }

    public T Add(T item)
    {
        _items.Add(item);
        return item;
    }

    public T? Update(T item)
    {
        Delete(item);
        _items.Add(item);
        return item;
    }

    public bool Delete(T item)
    {
        return _items.Remove(item);
    }
}
