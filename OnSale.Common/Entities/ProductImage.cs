using OnSale.Common.Business;
using System.ComponentModel;

namespace OnSale.Common.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string UrlImage { get; set; }

        [DisplayName("Imagen")]
        public string UrlImageFull 
        {
            get
            {
                string image = "d844c6c4-c929-4518-abeb-e900ac95ac53";
                if (UrlImage == image)
                {
                    return $"{Constants.Path.PathImageEmpty}";
                }
                else if (string.IsNullOrEmpty(UrlImage))
                {
                    return $"{Constants.Path.PathNoImage}";
                }
                return UrlImage;
            }

        }

        //[Display(Name = "Imagen")]
        //public Guid ImageId { get; set; }

        //[Display(Name = "Imagen")]
        //public string ImageFullPath
        //{
        //    get
        //    {
        //        var guid = Guid.Parse("d844c6c4-c929-4518-abeb-e900ac95ac53");
        //        if(ImageId == guid)
        //        {
        //            return $"{Constants.Path.PathImageEmpty}";
        //        }
        //        else if (ImageId == Guid.Empty)
        //        {
        //            return $"{Constants.Path.PathNoImage}";
        //        }
        //        return $"{Constants.URL_BASE_BLOB}/products/{ImageId}";
        //    }
        //}

    }
}
