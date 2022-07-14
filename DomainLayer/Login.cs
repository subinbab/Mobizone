using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    [Table("Login")]
    public class Login
    {
        [Key]
        public int id { get; set; }
       
        public string username { get; set; }
      
        public string password { get; set; }
        public int rolesId { get; set; }
        public Roles roles { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public string modifiedBy { get; set; }
        public string? sessionId { get; set; }
        public int IsActive { get; set; }
    }
}
