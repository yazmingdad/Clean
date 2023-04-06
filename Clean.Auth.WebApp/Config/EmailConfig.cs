using Clean.Email;
using Clean.Email.MailKit;
using Clean.Infrastructure.CleanDb.Models;
using Microsoft.EntityFrameworkCore;

namespace Clean.Auth.WebApp.Config
{
    public static class EmailConfig
    {
        public static void SetupEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMailService, MailKitService>();
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        }
    }
}
