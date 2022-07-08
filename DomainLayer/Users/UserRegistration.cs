using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.Users
{
    [Table("UserRegistration")]
    public class UserRegistration
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage ="This field is required")]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required")]
       
        /*[Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }*/
        [Display(Name = "Created On")]
        [Column("CreatedOn")]
        public DateTime createdOn { get; set; }
        [Column("CreatedBy", TypeName = "nvarchar")]
        [MaxLength(150)]
        public string createdBy { get; set; }
        [Column("ModifiedOn")]
        public DateTime modifiedOn { get; set; }
        [Column("ModifiedBy", TypeName = "nvarchar")]
        [MaxLength(150)]
        public string modifiedBy { get; set; }
        public ICollection<Address> ? address { get; set; }
        public int IsActive { get; set; }
    }
}
