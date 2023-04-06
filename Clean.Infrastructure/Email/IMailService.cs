using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.Email
{
    internal interface IMailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
