using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }

}
