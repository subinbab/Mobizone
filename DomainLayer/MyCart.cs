using DomainLayer.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    [Table("MyCart")]
    public class MyCart
    {
        public int id { get; set; }
        public string? sessionId { get; set; }
        public ICollection<CartDetails> cartDetails { get; set; }

        public int usersId { get; set; }
        public UserRegistration users { get; set; }
    }
}
