﻿using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public ProductEntity Product { get; set; }

        public float Quantity { get; set; }

        public decimal Price { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => (decimal)Quantity * Price;
    }
}
