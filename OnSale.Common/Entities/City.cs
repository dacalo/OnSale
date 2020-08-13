using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnSale.Common.Entities
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [NotMapped]
        [JsonIgnore]
        public int IdDepartment { get; set; }
    }
}
