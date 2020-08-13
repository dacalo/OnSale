using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Web.Data;
using OnSale.Web.Helpers;
using OnSale.Web.Models;

namespace OnSale.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] Pagination pagination)
        {
            var queryable = _context.Products.Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive).AsQueryable();
            await HttpContext.InsertParametersPagination(queryable, pagination.RecordsPage);
            var products = await queryable.Paginate(pagination).ToListAsync();
            return Ok(products);
        }

    }
}
