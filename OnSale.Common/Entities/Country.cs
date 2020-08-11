using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage ="El campo {0} debe contener menos de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }

        [DisplayName("Número de Estados")]
        public int DepartmentsNumber => Departments == null ? 0 : Departments.Count;
    }
}
