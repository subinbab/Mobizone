using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.ProductModel
{
    [Table("Specificatiion")]
    public class Specificatiion
    {
        [Key]
        public int id { get; set; }
        [NotMapped]
        [Display(Name = "Ram")]
        public List<string> ram { get; set; }
        public ICollection<Ram> rams { get; set; }
        public ICollection<Storage> storages { get; set; }
        [NotMapped]
        [Display(Name = " Storage")]
        public List<string> storage { get; set; }
        [Required]
        [Display(Name = " Sim Type")]
        public string simType { get; set; }
        [Required]
        [Display(Name = " Processor")]
        public string processor { get; set; }
        [Required]
        [Display(Name = " Processor  Core")]
        public string core { get; set; }
        [Display(Name = "Operating System")]
        public string os { get; set; }
        [Display(Name = " Cam Features")]
        public int? camFeatures { get; set; }
        public int IsActive { get; set; }
    }
}
