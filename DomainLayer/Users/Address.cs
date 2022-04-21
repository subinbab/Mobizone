using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Users
{
    public class Address
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string phoneNumber { get; set; }
    }
}
