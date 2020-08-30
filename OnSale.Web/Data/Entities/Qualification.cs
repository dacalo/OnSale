using OnSale.Common.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnSale.Web.Data.Entities
{
    public class Qualification
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime DateLocal => Date.ToLocalTime();

        [JsonIgnore]
        public Product Product { get; set; }

        public User User { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [DisplayName("Calificación")]
        public float Score { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Observaciones")]
        public string Remarks { get; set; }
    }
}
