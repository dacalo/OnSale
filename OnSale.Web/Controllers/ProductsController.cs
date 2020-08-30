using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Common.Business;
using OnSale.Common.Entities;
using OnSale.Web.Data;
using OnSale.Web.Data.Entities;
using OnSale.Web.Helpers;
using OnSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private const string _folder = "products";
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IMapper _mapper;

        public ProductsController(
            DataContext context,
            IBlobHelper blobHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IMapper mapper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Qualifications)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ProductViewModel model = new ProductViewModel
            {
                Categories = _combosHelper.GetComboCategories(),
                IsActive = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = await _converterHelper.ToProductAsync(model, true);

                    if (model.ImageFile != null)
                    {
                        string urlImage = await _blobHelper.SaveFile(model.ImageFile, _folder);
                        product.ProductImages = new List<ProductImage>
                        {
                            new ProductImage { UrlImage = urlImage }
                        };
                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains(Constants.TextString.MessageContains))
                        ModelState.AddModelError(string.Empty, Constants.TextString.MessageErrorDuplicate);
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Categories = _combosHelper.GetComboCategories();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Product product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound();

            ProductViewModel model = _converterHelper.ToProductViewModel(product);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = await _converterHelper.ToProductAsync(model, false);

                    if (model.ImageFile != null)
                    {
                        string urlImage = await _blobHelper.EditFile(model.ImageFile, _folder, product.ImageFullPath);
                        if (product.ProductImages == null)
                            product.ProductImages = new List<ProductImage>();

                        product.ProductImages.Add(new ProductImage { UrlImage = urlImage });
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains(Constants.TextString.MessageContains))
                        ModelState.AddModelError(string.Empty, Constants.TextString.MessageErrorDuplicate);
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Categories = _combosHelper.GetComboCategories();
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            ProductEntity product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound();

            try
            {
                foreach (var file in product.ProductImages)
                {
                    await _blobHelper.DeleteFile(file.UrlImage, _folder);
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            Product product = await _context.Products
                .Include(c => c.Category)
                .Include(c => c.ProductImages)
                .Include(c => c.Qualifications)
                .ThenInclude(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
                return NotFound();

            Product product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            AddProductImageViewModel model = new AddProductImageViewModel { ProductId = product.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == model.ProductId);
                if (product == null)
                    return NotFound();

                try
                {
                    string urlImage = await _blobHelper.SaveFile(model.ImageFile, _folder);
                    if (product.ProductImages == null)
                        product.ProductImages = new List<ProductImage>();

                    product.ProductImages.Add(new ProductImage { UrlImage = urlImage });
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = product.Id });
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
                return NotFound();

            ProductImage productImage = await _context.ProductImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null)
                return NotFound();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.ProductImages.FirstOrDefault(pi => pi.Id == productImage.Id) != null);
            await _blobHelper.DeleteFile(productImage.UrlImage, _folder);
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details) , new { id = product.Id });
        }

    }
}
