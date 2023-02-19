using Clean.Infrastructure.Identity.Services;

namespace Clean.Auth.WebApp.Config
{
    public static class ServicesConfig
    {
        public static void SetupServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddTransient<IUserService, UserService>();
        }
    }
}
