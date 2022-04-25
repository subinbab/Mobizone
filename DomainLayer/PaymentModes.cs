using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer
{
    public enum PaymentModes : int
    {
        cashOnDelivery = 1
    }
    public class PaymentMode
    {
        public int id { get; set; }
        public PaymentModes mode { get; set; }
        public bool IsChecked { get; set; }
    }


}
