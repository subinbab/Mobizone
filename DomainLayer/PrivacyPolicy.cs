using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("PrivacyPolicy")]
    public class PrivacyPolicy
    {
        [Key]
        [Column("id", Order = 1)]
        public int id { get; set; }
        [Column("content", TypeName = "nvarchar", Order = 2)]
        [MaxLength(500)]
        [Required(ErrorMessage = "This field is required")]
        public string content { get; set; }
        public int IsActive { get; set; }
    }
}
