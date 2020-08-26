using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnSale.Common.Business;
using OnSale.Common.Enums;
using OnSale.Web.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string RFC { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [DisplayName("Domicilio")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Imagen")]
        public string ImageId { get; set; }

        [Display(Name = "Imagen")]
        public string ImageFullPath => ImageId == string.Empty
            ? $"{Constants.Path.PathNoImage}"
            : $"{ImageId}";

        [Display(Name = "Imagen")]
        [SizeFileValidation(sizeMaximum: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "País")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un país.")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Estado")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado.")]
        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Ciudad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una ciudad.")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }
    }

}
