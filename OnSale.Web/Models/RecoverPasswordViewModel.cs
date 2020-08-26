using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [DisplayName("Correo")]
        public string Email { get; set; }
    }
}
