using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("Contact")]
   public  class Contact
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string shopName { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string district { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string pincode { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string phoneNumber { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }

    }
}
