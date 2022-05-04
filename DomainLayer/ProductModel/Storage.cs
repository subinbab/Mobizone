using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.ProductModel
{
    public class Storage
    {
        public int id { get; set; }
        public string storage { get; set; }
        public int specificationid { get; set; }
        public Specificatiion specification { get; set; }
    }
}
