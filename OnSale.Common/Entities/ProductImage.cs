using OnSale.Common.Business;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Display(Name = "Imagen")]
        public Guid ImageId { get; set; }

        [Display(Name = "Imagen")]
        public string ImageFullPath
        {
            get
            {
                var guid = Guid.Parse("d844c6c4-c929-4518-abeb-e900ac95ac53");
                if(ImageId == guid)
                {
                    return $"{Constants.Path.PathImageEmpty}";
                }
                else if (ImageId == Guid.Empty)
                {
                    return $"{Constants.Path.PathNoImage}";
                }
                return $"{Constants.URL_BASE_BLOB}/products/{ImageId}";
            }
        }

        


    }
}
