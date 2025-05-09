using System.Linq.Expressions;

namespace ManagerDish.Extensions
{
    public static class QueryableExtensions
    {
            public static Task<PagedList<T>> ToPageListAsync<T>(this IQueryable<T> source,Expression<Func<T, object>> order, int pageNumber, int pageSize)
            {
                source.OrderBy(order);
                return PagedList<T>.CreateAsync(source, pageNumber, pageSize);
            }
        }
}
