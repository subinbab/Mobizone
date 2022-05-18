using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer
{
    public class CartDetails
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int? quantity { get; set; }
        public int? price { get; set; }
    }
}
