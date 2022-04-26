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
        public string phoneNumber { get; set; }
        public string email { get; set; }

    }
}
