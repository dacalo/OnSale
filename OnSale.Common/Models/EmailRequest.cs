using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
