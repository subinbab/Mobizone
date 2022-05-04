using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainLayer.ProductModel
{
    public class Ram
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string ram { get; set; }
        public int? specificatiionid { get; set; }
        public Specificatiion specificatiion { get; set; }
    }
}
