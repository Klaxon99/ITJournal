using System.Linq.Expressions;

namespace ITJournal.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> expression)
        {
            return condition ? query.Where(expression) : query;
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int? skip = null, int? take = null)
        {
            query = skip != null ? query.Skip(skip.Value) : query;
            query = take != null ? query.Take(take.Value) : query;

            return query;
        }
    }
}
