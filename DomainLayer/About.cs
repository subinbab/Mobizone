using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainLayer
{
    [Table("About")]
     public class About
    {
        [Key]
        [Column("id", Order = 1)]
        public int id { get; set; }
        [Column("content", Order = 2)]
        public string content { get; set; }
    }
}
