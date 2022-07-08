using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.ProductModel
{
    [Table("Storage")]
    public class Storage
    {
        public int id { get; set; }
        public string storage { get; set; }
        public int specificationid { get; set; }
        public Specificatiion specification { get; set; }
        public int IsActive { get; set; }
    }
}
