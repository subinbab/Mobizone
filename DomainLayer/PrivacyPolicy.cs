﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer
{
    [Table("Privacy")]
    public class PrivacyPolicy
    {
        [Key]
        [Column("id", Order = 1)]
        public int id { get; set; }
        [Column("content", Order = 2)]
        [MaxLength]
        [Required(ErrorMessage = "This Field is Required")]
        public string content { get; set; }
    }
}
