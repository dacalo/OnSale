using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametersPagination<T>(
            this HttpContext httpContext,
            IQueryable<T> queryable,
            int recordsPage)
        {
            double quantity = await queryable.CountAsync();
            double quantityPages = Math.Ceiling(quantity / recordsPage);
            httpContext.Response.Headers.Add("quantityPages", quantityPages.ToString());
        }
    }
}
