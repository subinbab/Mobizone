using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("Privacy")]
    public class PrivacyPolicy
    {
        [Key]
        [Column("id", Order = 1)]
        public int id { get; set; }
        [Column("content", TypeName = "nvarchar", Order = 2)]
        [MaxLength(150)]
        [Required(ErrorMessage = "This Field is Required")]
        public string content { get; set; }
    }
}
