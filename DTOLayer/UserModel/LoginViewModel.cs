using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainLayer.Users
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name ="Username")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "This field should not contain any numbers or special characters")]
        public string username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string password { get; set; }
        public string sessionId { get; set; }
        public string url { get; set; }
    }
}
