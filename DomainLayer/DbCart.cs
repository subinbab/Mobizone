using DomainLayer.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("DbCart")]
    public class DbCart
    {
        [Column("Id")]
        public int id { get; set; }
        public string? sessionId { get; set; }
        [ForeignKey("DbCartId")]
        public ICollection<CartDetails> cartDetails { get; set; }
        public int usersId { get; set; }
        public UserRegistration users { get; set; }
        public int IsActive { get; set; }
    }
}
