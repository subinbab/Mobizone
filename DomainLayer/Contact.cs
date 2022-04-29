using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("Contact")]
   public  class Contact
    {
        [Key]
        public int id { get; set; }
        public string shopName { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string phoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }

    }
}
