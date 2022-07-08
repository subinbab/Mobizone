using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.Users
{
    [Table("Address")]
    public class Address
    {
        public int id { get; set; }
         [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Name")]
        public string name { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "District")]
        public string district { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "State")]
        public string state { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Country")]
        public string country { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Pincode")]
        public string pincode { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Phone number")]

        public string phoneNumber { get; set; }
        [Display(Name = "AdditionalInfo")]
        public string additionalInfo { get; set; }
        [NotMapped]
        public bool IsChecked { get; set; }
        public int IsActive { get; set; }

    }
}
