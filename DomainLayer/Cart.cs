using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer
{
    public class Cart
    {
        public int id { get; set; }
        public int sessionId { get; set; }
        public ICollection<CartDetails> cartDetails { get; set; }
    }
}
