using System.ComponentModel.DataAnnotations;

namespace DomainLayer
{

    public enum PaymentModes : int
    {

        [Display(Name = "Cash on delivery")]
        CashOnDelivery = 1
 

    }
    public class PaymentMode
    {
        public int id { get; set; }
        [Display(Name = "Cash on delivery")]
        public PaymentModes mode { get; set; }
        public bool IsChecked { get; set; }
    }


}
