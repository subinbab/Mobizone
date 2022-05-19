using DomainLayer.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    public class Cart
    {
        public int id { get; set; }
        public string sessionId { get; set; }
        public ICollection<CartDetails> cartDetails { get; set; }
        [ForeignKey("userId")]
        public UserRegistration? userId { get; set; }
    }
}
