using Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositiories
{
    public class SqlServerBaseRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly ApplicationDbContext _dbContext;

        public SqlServerBaseRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public T[] GetItems(int? offset = null, int? limit = null, Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>? orderBy = null, bool? destinct = null)
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

            return items.ToArray();
        }

        public int Count(Expression<Func<T, bool>>? predicate = null)
        {
            var set = _dbContext.Set<T>();
            return predicate == null
                ? set.Count() 
                : set.Count(predicate);
        }

        public T? SingleOrDefault(Expression<Func<T, bool>>? predicate)
        {
            var set = _dbContext.Set<T>();

            return predicate == null
                ? set.SingleOrDefault()
                : set.SingleOrDefault(predicate);
        }

        public T Add(T item)
        {
            var set = _dbContext.Set<T>();
            set.Add(item);
            _dbContext.SaveChanges();
            return item;
        }

        public T Update(T item)
        {
            var set = _dbContext.Set<T>();
            set.Update(item);
            _dbContext.SaveChanges();
            return item;
        }

        public bool Delete(T item)
        {
            var set = _dbContext.Set<T>();
            set.Remove(item);
            return _dbContext.SaveChanges() > 0;
        }
    }
}
