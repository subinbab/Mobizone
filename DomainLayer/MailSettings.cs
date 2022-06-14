using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer
{
    public class MailSettings
    {

        public string Mail = "darwincabral9@gmail.com";
        public string DisplayName = "Darwin Cabral";
        public string Password = "ipkbvycsebplzrcm";
        public string Host = "smtp.gmail.com";
        public int Port = 587;


    }
}
