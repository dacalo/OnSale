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

        //[Display(Name = "Imagen")]
        //public string ImageFullPath => ImageId == string.Empty
        //    ? $"{Constants.Path.PathNoImage}"
        //    : $"{ImageId}";

        [Display(Name = "Tipo Usuario")]
        public UserType UserType { get; set; }

        public City City { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {RFC}";

        [Display(Name = "Login Type")]
        public LoginType LoginType { get; set; }

        public string ImageFacebook { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath
        {
            get
            {
                if (LoginType == LoginType.Facebook && string.IsNullOrEmpty(ImageFacebook) ||
                    LoginType == LoginType.OnSale && ImageId == string.Empty)
                {
                    return $"{Constants.Path.PathNoImage}";
                }

                if (LoginType == LoginType.Facebook)
                {
                    return ImageFacebook;
                }

                return $"{ImageId}";
            }
        }

        [DisplayFormat(DataFormatString = "{0:N4}")]
        public double Latitude { get; set; }

        [DisplayFormat(DataFormatString = "{0:N4}")]
        public double Logitude { get; set; }

    }
}
