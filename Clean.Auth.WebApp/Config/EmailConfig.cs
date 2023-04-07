
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.Email;
using Clean.Infrastructure.Email.MailKit;
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
