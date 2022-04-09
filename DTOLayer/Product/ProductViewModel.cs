using DomainLayer.ProductModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.Product
{
    public class ProductViewModel
    {
        [Required]
        [DisplayName("Type")]
        public string productType { get; set; }
        [DisplayName("Brand")]
        [Required]
        public string productBrand { get; set; }
        [DisplayName("Name")]
        [Required]
        public string name { get; set; }
        [DisplayName("Model")]
        [Required]
        public string model { get; set; }
        [DisplayName("Price")]
        [Required]
        public int price { get; set; }
        public List<IFormFile> imageFile { get; set; }
        [DisplayName("Quantity")]
        [Required]
        public int quantity { get; set; }
        public Specificatiion specs { get; set; }
        [DisplayName("Description")]
        public string description { get; set; }
    }
}
