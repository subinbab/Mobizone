using DomainLayer.ProductModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BusinessObjectLayer.ProductOperations
{
    [Table("ProductSubPart")]
    public class ProductSubPart
    {
        [Key]
        public int id { get; set; }
        public int productId { get; set; }
        public ProductEntity product { get; set; }

        public int ramId { get; set; }
        [ForeignKey("ramId")]
        [NotMapped]
        public Ram ram { get; set; }

        public int? storageId { get; set; }
        [ForeignKey("storageId")]
        [NotMapped]
        public Storage storage { get; set; }

        public Color color { get; set; }

        public int quantity { get; set; }
        public int price { get; set; }
        public int IsActive { get; set; }
        public ICollection<Images> images { get; set; }

    }
}
