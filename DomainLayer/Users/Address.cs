using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.Users
{
    public class Address
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string district { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string pincode { get; set; }
        [Required]
        public string phoneNumber { get; set; }

        public string additionalInfo { get; set; }
        [NotMapped]
        public bool IsChecked { get; set; }

    }
}
