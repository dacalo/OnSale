using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Requests
{
    public class EmailRequest
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del campo {0} es inválido.")]
        public string Email { get; set; }
    }
}
