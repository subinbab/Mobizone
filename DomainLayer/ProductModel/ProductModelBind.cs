using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.ProductModel
{
    public class ProductModelBind
    {
        public ProductEntity product { get; set; }
        public Images image { get; set; }
        public int IsActive { get; set; }
    }
}
