using Microsoft.AspNetCore.Identity;
using OnSale.Common.Business;
using OnSale.Common.Entities;
using OnSale.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string RFC { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Imagen")]
        public string ImageId { get; set; }

        [Display(Name = "Imagen")]
        public string ImageFullPath => ImageId == string.Empty
            ? $"{Constants.Path.PathNoImage}"
            : $"{ImageId}";

        [Display(Name = "Tipo Usuario")]
        public UserType UserType { get; set; }

        public City City { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {RFC}";
    }
}
