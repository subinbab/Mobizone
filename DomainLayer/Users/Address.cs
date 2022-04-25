using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
<<<<<<< HEAD
        public string Email { get; set; }
=======
        public string additionalInfo { get; set; }
        [NotMapped]
        public bool IsChecked { get; set; }
>>>>>>> 919e8df021298699746392ef1e2d9b6190874b71
    }
}
