using OnSale.Web.Models;
using System.Linq;

namespace OnSale.Web.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, Pagination pagination)
        {
            return queryable.Skip((pagination.Page - 1) * pagination.RecordsPage).Take(pagination.RecordsPage);
        }
    }
}
