using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.ProductModel
{
    [Table("ProductModel")]
    public class ProductEntity
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }
        [Required]
        [Column("Type",TypeName ="nvarchar",Order =1)]
        [MaxLength(150)]
        public string productType { get; set; }
        [Required]
        [Column("Brand", TypeName = "nvarchar", Order = 2)]
        [MaxLength(150)]
        public string productBrand { get; set; }
        [Required]
        [Column("Name", TypeName = "nvarchar", Order = 3)]
        [MaxLength(150)]
        public string name { get; set; }
        [Required]
        [Column("Model", TypeName = "nvarchar", Order = 4)]
        [MaxLength(150)]
        public string model { get; set; }
        [Required]
        public int price { get; set; }
        public ICollection<Images> images { get; set; }
        public int quantity { get; set; }
        public Specificatiion? specs { get; set; }
        public string description { get; set; }
        
        private int IsActive { get; set; }
        public ProductStatus status { get; set; }
    }
}
