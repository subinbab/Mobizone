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
        [Display]
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }
        [Required]
        [Column("Type",TypeName ="nvarchar",Order =1)]
        [MaxLength(150)]
        [Display(Name = "Type")]
        public string productType { get; set; }
        [Required]
        [Column("Brand", TypeName = "nvarchar", Order = 2)]
        [MaxLength(150)]
        [Display(Name = "Brand")]
        public string productBrand { get; set; }
        [Required]
        [Column("Name", TypeName = "nvarchar", Order = 3)]
        [MaxLength(150)]
        [Display(Name = "Name")]
        public string name { get; set; }
        [Required]
        [Column("Model", TypeName = "nvarchar", Order = 4)]
        [MaxLength(150)]
        [Display(Name = "Model")]
        public string model { get; set; }
        [Required]
        [Column("Price",TypeName ="decimal(18,2)",Order =5)]
        [Display(Name = "Price")]
        public int price { get; set; }
        
        public ICollection<Images> images { get; set; }
        public ICollection<Color> colors { get; set; }
        [Column("Quantity",Order=7)]
        [Display(Name = "Quantity")]
        public int quantity { get; set; }

        public int specsId { get; set; }
        [Column("Specifications",Order=8)]
        public Specificatiion? specs { get; set; }
        [Column("Description",TypeName ="nvarchar",Order =9)]
        [MaxLength(250)]
        [Display(Name = "Description")]
        public string description { get; set; }
        [Column("PurchasedNumber", TypeName = "nvarchar", Order = 10)]
        public int? purchasedNumber { get; set; }
        public int IsActive { get; set; }
        public ProductStatus status { get; set; }
    }
}
