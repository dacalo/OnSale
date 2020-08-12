using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnSale.Common.Entities;
using OnSale.Common.Enums;
using OnSale.Web.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Models
{
    public class ProductViewModel : Product
    {
        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [SizeFileValidation(sizeMaximum: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile ImageFile { get; set; }
    }

}
