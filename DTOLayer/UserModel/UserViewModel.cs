using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.UserModel
{
  public  class UserViewModel
    {
        [Required (ErrorMessage = "This Field is Required")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$",ErrorMessage ="This field should not contain any numbers or special characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "This field should not contain any numbers or special characters")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z\s]+\.[a-zA-Z\s.]+$",ErrorMessage="Invalid Email format")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Your password must be at least 8 characters long")]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Your password must be at least 8 characters long")]
        public string ConfirmPassword { get; set; }
    }
}
