using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.UserModel
{
    public class ForgetPasswordViewModel
    {
        public int userId { get; set; }

        [Required(ErrorMessage = "*Email Address is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z\s]+\.[a-zA-Z\s.]+$", ErrorMessage = "Invalid Email format")]
        [Display(Name = "Registered Email Address")]
        public string email { get; set; }
        public bool emailSent { get; set; }
    }
}
