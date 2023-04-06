using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.Email.MailKit
{
    internal class MailKitService:IMailService
    {
        readonly ConcurrentQueue<SmtpClient> _clients = new ConcurrentQueue<SmtpClient>();

        private readonly MailSettings _mailSettings;
        public MailKitService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var client = GetOrCreateSmtpClient();
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(message.Destination));
                email.Subject = message.Subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = message.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch
            {
                throw;
            }
            finally
            {
                _clients.Enqueue(client);
            }
        }

        private SmtpClient GetOrCreateSmtpClient()
        {
            SmtpClient client = null;
            if (_clients.TryDequeue(out client))
            {
                return client;
            }
            client = new SmtpClient();
            return client;
        }
    }
}
