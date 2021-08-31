using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnSale.Common.Entities;
using OnSale.Common.Enums;
using OnSale.Web.Data.Entities;
using OnSale.Web.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Models
{
    public class ProductViewModel : Product
    {
        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CategoryId { get; set; }

        [DisplayName("Precio")]
        [MaxLength(12, ErrorMessage ="El campo {0} debe ser menor a {1} números")]
        [RegularExpression(@"^\d+([\.\,]?\d+)?$", ErrorMessage = "Use solo números y . ó , para ingresar decimales")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PriceString { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [DisplayName("Imagen")]
        [SizeFileValidation(sizeMaximum: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile ImageFile { get; set; }
    }

}
