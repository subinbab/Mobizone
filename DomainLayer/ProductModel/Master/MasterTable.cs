using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.ProductModel.Master
{
    [Table("Master")]
    public class MasterTable
    {
  
        [Key]
        [Column("id", Order = 1)]
        public int id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Column("MasterData",Order =2,TypeName ="nvarchar")]
        [MaxLength(150)]
        [Display(Name = "MasterData")]
        public string masterData { get; set; }
        [Column("PerantId",Order =3)]
        public int? parantId { get; set; }
        public int IsActive { get; set; }
    }
}
