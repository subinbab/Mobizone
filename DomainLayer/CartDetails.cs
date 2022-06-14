using DomainLayer.ProductModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    [Table("CartDetails")]
    public class CartDetails
    {
        public int id { get; set; }
        public int productId { get; set; }
        public ProductEntity product { get; set; }
        public int? quantity { get; set; }
        public int? price { get; set; }
    }
}
