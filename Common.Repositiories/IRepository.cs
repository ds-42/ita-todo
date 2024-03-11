using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories;

public interface IRepository<T> where T : class, new()
{
    T[] GetItems(
        int? offset = null, 
        int? limit = null, 
        Expression<Func<T, bool>>? predicate = null, 
        Expression<Func<T, object>>? orderBy = null,
        bool? destinct = null);

    int Count(Expression<Func<T, bool>>? predicate = null);

    T? SingleOrDefault(Expression<Func<T, bool>>? predicate);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    T Add(T item);
    T Update(T item);
    bool Delete(T item);
}
