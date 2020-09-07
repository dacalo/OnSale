using OnSale.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnSale.Web.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [Display(Name = "Fecha")]
        public DateTime DateLocal => Date.ToLocalTime();

        [Display(Name = "Usuario")]
        public User User { get; set; }

        [Display(Name = "Status Pedido")]
        public OrderStatus OrderStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [Display(Name = "Fecha Expedición")]
        public DateTime? DateSent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [Display(Name = "Fecha Expedición")]
        public DateTime? DateSentLocal => DateSent?.ToLocalTime();

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [Display(Name = "Fecha Confirmación")]
        public DateTime? DateConfirmed { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [Display(Name = "Fecha Confirmación")]
        public DateTime? DateConfirmedLocal => DateSent?.ToLocalTime();

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string Remarks { get; set; }

        [Display(Name = "Método de Pago")]
        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public int Lines => OrderDetails == null ? 0 : OrderDetails.Count;

        public float Quantity => OrderDetails == null ? 0 : OrderDetails.Sum(od => od.Quantity);

        public decimal Value => OrderDetails == null ? 0 : OrderDetails.Sum(od => od.Value);
    }

}
