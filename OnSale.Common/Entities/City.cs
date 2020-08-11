using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Entities
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
