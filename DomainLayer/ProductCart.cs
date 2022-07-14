using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("ProductCart")]
    public class ProductCart
    {
        public int id { get; set; }
        public string sessionId { get; set; }
        public ICollection<CartDetails> cartDetails { get; set; }

        public int? usersId { get; set; }

    }
}
