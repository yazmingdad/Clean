using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
