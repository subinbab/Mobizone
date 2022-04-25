using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer
{
    public class CheckOutDetails
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public RoleTypes? cancelRequested { get; set; }
        public OrderStatus status { get; set; }
    }
}
