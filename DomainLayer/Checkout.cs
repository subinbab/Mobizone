using DomainLayer.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    public class Checkout
    {
        [Key]
        public int id { get; set; }
        public int orderId { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }
        public int quatity { get; set; }
        public int price { get; set; }
        [BindProperty]
        public int paymentModeId { get; set; }
        [BindProperty]
        public int addressId { get; set; }
        public OrderStatus status { get; set; }
        public RoleTypes? cancelRequested { get; set; }
    }
}
