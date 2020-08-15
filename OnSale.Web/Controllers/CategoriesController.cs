﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Common.Business;
using OnSale.Common.Entities;
using OnSale.Web.Data;
using OnSale.Web.Helpers;
using OnSale.Web.Models;
using System;
using System.Threading.Tasks;

namespace OnSale.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private const string _folder = "categories";
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public CategoriesController(
            DataContext context,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string urlImage = string.Empty;

                if (model.ImageFile != null)
                    urlImage = await _blobHelper.SaveFile(model.ImageFile, _folder);

                try
                {
                    Category category = _converterHelper.ToCategory(model, urlImage, true);
                    _context.Add(category);
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

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            CategoryViewModel model = _converterHelper.ToCategoryViewModel(category);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string urlImage = model.UrlImage;

                if (model.ImageFile != null)
                    urlImage = await _blobHelper.EditFile(model.ImageFile, _folder, urlImage);

                try
                {
                    Category category = _converterHelper.ToCategory(model, urlImage, false);
                    _context.Update(category);
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

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
                return NotFound();

            try
            {
                await _blobHelper.DeleteFile(category.UrlImage, _folder);
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
