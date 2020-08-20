using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnSale.Common.Entities
{
    public class Department
    {
        public int Id { get; set; }
        
        [MaxLength(50, ErrorMessage = "El campo {0} ")]
        [Required(ErrorMessage = "El campo {0} debe contener menos de {1} caracteres.")]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }

        [DisplayName("Número de Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;

        [NotMapped]
        [JsonIgnore]
        public int IdCountry { get; set; }
    }
}
