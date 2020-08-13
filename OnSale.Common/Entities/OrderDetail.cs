using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Display(Name = "Producto")]
        public Product Product { get; set; }

        [Display(Name = "Cantidad")]
        public float Quantity { get; set; }

        [Display(Name = "Precio Unitario")]
        public decimal Price { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string Remarks { get; set; }

        [Display(Name = "Precio Total")]
        public decimal Value => (decimal)Quantity * Price;
    }
}
