using DomainLayer.ProductModel;
using DomainLayer.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("Order")]
    public class Checkout
    {
        [Key]
        public int id { get; set; }
        public int orderId { get; set; }
        public int userId { get; set; }
        public UserRegistration user { get; set; }
        

        public int quantity { get; set; }
        public int productId { get; set; }
        public ProductEntity product { get; set; }

        public int price { get; set; }
        [BindProperty]
        public int paymentModeId { get; set; }
        [BindProperty]
        public int addressId { get; set; }
        public Address address { get; set; }
        public OrderStatus status { get; set; }
        public RoleTypes? cancelRequested { get; set; }
        [NotMapped]
        public List<Address> addressList { get; set; }
        public DateTime? createdOn { get; set; }
        [Column("CreatedBy", TypeName = "nvarchar")]
        [MaxLength(150)]
        public string? createdBy { get; set; }
        [Column("ModifiedOn")]
        public DateTime? modifiedOn { get; set; }
        [Column("ModifiedBy", TypeName = "nvarchar")]
        [MaxLength(150)]
        public string? modifiedBy { get; set; }
        public int IsActive { get; set; }
    }
}
