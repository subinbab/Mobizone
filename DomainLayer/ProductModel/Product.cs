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
        [Column("Price",TypeName ="decimal(18,2)",Order =5)]
        
        public int price { get; set; }
        
        public ICollection<Images> images { get; set; }
        [Column("Quantity",Order=7)]
        public int quantity { get; set; }
        [Column("Specifications",Order=8)]
        public Specificatiion? specs { get; set; }
        [Column("Description",TypeName ="nvarchar",Order =9)]
        [MaxLength(100)]
        public string description { get; set; }
        
        private int IsActive { get; set; }
        public ProductStatus status { get; set; }
    }
}
