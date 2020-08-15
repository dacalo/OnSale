using OnSale.Common.Business;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        public string UrlImage { get; set; }

        [DisplayName("Imagen")]
        public string UrlImageFull => UrlImage == string.Empty
            ? $"{Constants.Path.PathNoImage}"
            : UrlImage;

        //[Display(Name = "Imagen")]
        //public Guid ImageId { get; set; }

        //[Display(Name = "Imagen")]
        //public string ImageFullPath => ImageId == Guid.Empty
        //    ? $"{Constants.Path.PathNoImage}"
        //    : $"{Constants.URL_BASE_BLOB}/categories/{ImageId}";
    }
}
