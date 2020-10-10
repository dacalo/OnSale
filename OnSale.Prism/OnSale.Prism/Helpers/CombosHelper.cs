using OnSale.Common.Models;
using System.Collections.Generic;

namespace OnSale.Prism.Helpers
{
    public class CombosHelper : ICombosHelper
    {

        public IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            return  new List<PaymentMethod>
            {
                new PaymentMethod { Id = 1, Name = Languages.Cash },
                new PaymentMethod { Id = 2, Name = Languages.CreditCard }
            };
        }
    }
}
