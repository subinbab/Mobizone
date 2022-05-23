using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    public class DbCart
    {
        [Column("Id")]
        public int id { get; set; }
        public string sessionId { get; set; }
        [ForeignKey("DbCartId")]
        public ICollection<CartDetails> cartDetails { get; set; }
    }
}
