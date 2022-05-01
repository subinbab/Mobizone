using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.ProductModel
{
    public class Specificatiion
    {
        [Key]
        public int id { get; set; }
        [NotMapped]
        public List<string> ram { get; set; }
        public ICollection<Ram>? rams { get; set; }
        public ICollection<Storage>? storages { get; set; }
        [NotMapped]
        public List<string> storage { get; set; }
        [Required]
        public string simType { get; set; }
        [Required]
        public string processor { get; set; }
        [Required]
        public string core { get; set; }
    
        public string os { get; set; }
   
        public int? camFeatures { get; set; }
    }
}
