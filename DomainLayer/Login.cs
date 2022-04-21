using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainLayer
{
    public class Login
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int roleId { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public string modifiedBy { get; set; }
    }
}
