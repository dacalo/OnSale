using Microsoft.AspNetCore.Http;
using OnSale.Common.Entities;
using OnSale.Common.Enums;
using OnSale.Web.Validations;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Models
{
    public class CategoryViewModel : Category
    {
        [Display(Name = "Imagen")]
        [SizeFileValidation(sizeMaximum: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile ImageFile { get; set; }
    }

}
