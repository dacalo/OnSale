using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnSale.Common.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DisplayName("Precio")]
        public decimal Price { get; set; }

        [DisplayName("Activo")]
        public bool IsActive { get; set; }

        [DisplayName("Destacado")]
        public bool IsStarred { get; set; }

        [DisplayName("Categoría")]
        public Category Category { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        [DisplayName("Imágenes del Producto")]
        public int ProductImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        //TODO: Pending to put the correct paths
        [Display(Name = "Imagen")]
        public string ImageFullPath => ProductImages == null || ProductImages.Count == 0
            ? $"https://localhost:51672/images/noimage.png"
            : ProductImages.FirstOrDefault().ImageFullPath;
    }

}
