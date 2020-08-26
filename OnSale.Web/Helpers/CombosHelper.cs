using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnSale.Common.Entities;
using OnSale.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace OnSale.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboCategories()
        {
            List<SelectListItem> list = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = $"{c.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una categoría...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCities(int departmentId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Department department = _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefault(d => d.Id == departmentId);
            if (department != null)
                list = department.Cities.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.Id}"
                })
                    .OrderBy(t => t.Text)
                    .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una Ciudad...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCountries()
        {
            List<SelectListItem> list = _context.Countries.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un País...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDepartments(int countryId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Country country = _context.Countries
                .Include(c => c.Departments)
                .FirstOrDefault(c => c.Id == countryId);
            if (country != null)
                list = country.Departments.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.Id}"
                })
                    .OrderBy(t => t.Text)
                    .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Estado...]",
                Value = "0"
            });

            return list;
        }

    }
}
