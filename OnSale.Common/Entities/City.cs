using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnSale.Common.Entities
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "EEl campo {0} debe contener menos de {1} caracteres.")]
        [Required(ErrorMessage = "El cacmpo {0} es obligatorio")]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [NotMapped]
        [JsonIgnore]
        public int IdDepartment { get; set; }
    }
}
