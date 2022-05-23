using DomainLayer.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DTOLayer.UserModel
{
    public class ResetPassword
    {
        public int ResetId { get; set; }
        public UserRegistration User { get; set; }
        [Required(ErrorMessage = "*Password is required")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [RegularExpression("[^ ]{8,16}", ErrorMessage = "Password should contain a minimum of 8 characters and a capital letter")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "*Password is required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("newPassword")]
        public string confirmPassword { get; set; }
    }
}
