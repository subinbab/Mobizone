using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public string modifiedBy { get; set; }
        public int IsActive { get; set; }
    }
}
