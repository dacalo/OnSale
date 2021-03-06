﻿using Microsoft.AspNetCore.Http;
using OnSale.Common.Enums;
using OnSale.Web.Validations;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Models
{
    public class AddProductImageViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Imagen")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [SizeFileValidation(sizeMaximum: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile ImageFile { get; set; }
    }

}
