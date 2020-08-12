﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Common.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [Display(Name = "Imagen")]
        public Guid ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Imagen")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:51672/images/noimage.png"
            : $"https://onsale.blob.core.windows.net/categories/{ImageId}";
    }
}