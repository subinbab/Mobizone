using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainLayer.ProductModel
{
    public class Specificatiion
    {
        [Key]
        public int id { get; set; }
        public int ram { get; set; }
        public int storage { get; set; }
        public string simType { get; set; }
        public string processor { get; set; }
        public string core { get; set; }
        public string os { get; set; }
        public int? camFeatures { get; set; }
    }
}
