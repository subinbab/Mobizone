using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailrequest);
    }
}
