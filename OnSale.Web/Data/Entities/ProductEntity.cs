using OnSale.Common.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnSale.Web.Data.Entities
{
    public class ProductEntity : Product
    {
        public ICollection<Qualification> Qualifications { get; set; }

        [DisplayName("Calificaciones Producto")]
        public int ProductQualifications => Qualifications == null ? 0 : Qualifications.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [DisplayName("Calificación")]
        public float Qualification => Qualifications == null || Qualifications.Count == 0 ? 0 : Qualifications.Average(q => q.Score);

    }
}
