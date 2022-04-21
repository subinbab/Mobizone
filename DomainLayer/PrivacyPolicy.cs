using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    [Table("Privacy")]
    public class PrivacyPolicy
    {
        public int id { get; set; }
        public string content { get; set; }
    }
}
