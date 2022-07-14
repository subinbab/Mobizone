using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    [Table("CheckOutDetails")]
    public class CheckOutDetails
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public RoleTypes? cancelRequested { get; set; }
        public OrderStatus status { get; set; }
        public int IsActive { get; set; }
    }
}
