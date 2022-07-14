using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainLayer
{
    [Table("AdminContact")]
    public class AdminContact
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Column("Shopname", TypeName = "nvarchar", Order = 1)]
        [MaxLength(50)]
        public string shopName { get; set; }
        [Required]
        [Column("Address", TypeName = "nvarchar", Order = 2)]
        [MaxLength(50)]
        public string address { get; set; }
        [Required]
        [Column("District", TypeName = "nvarchar", Order = 3)]
        [MaxLength(50)]
        public string district { get; set; }
        [Required]
        [Column("State", TypeName = "nvarchar", Order = 4)]
        [MaxLength(50)]
        public string state { get; set; }
        [Required]
        [Column("Country", TypeName = "nvarchar", Order = 5)]
        [MaxLength(50)]
        public string country { get; set; }
       
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        [Column("PhoneNumber", TypeName = "Bigint", Order = 7)]
        
        public long phoneNumber { get; set; }
        [Required]
        [Column("Pincode", TypeName = "nvarchar", Order = 6)]
        [MaxLength(6)]
        [RegularExpression(@"^[1-9]{1}[0-9]{5}$", ErrorMessage = "Invalid Pincode")]
        public string pincode { get; set; }
        [Required]
        [Column("email", TypeName = "nvarchar", Order = 8)]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }
        public int IsActive { get; set; }
    }
}
