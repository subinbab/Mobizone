using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.ProductModel
{
    [Table("Ram")]
    public class Ram
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string ram { get; set; }
        public int? specificatiionid { get; set; }
        public Specificatiion specificatiion { get; set; }
        public int IsActive { get; set; }
    }
}
