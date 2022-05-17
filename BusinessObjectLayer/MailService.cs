using DomainLayer;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
/*using System.Net.Mail;*/
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailrequest)
        {
            var Email = new MimeMessage();
            Email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            Email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            Email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();
            if (mailrequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach(var file in mailrequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                            

                    }
                }
            }
            builder.HtmlBody = mailrequest.Body;
            Email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(Email);
            smtp.Disconnect(true);
        }
    }
}
