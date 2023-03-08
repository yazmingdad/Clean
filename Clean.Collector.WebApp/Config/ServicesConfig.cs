
using Clean.Infrastructure.CleanDb.Services;

namespace Clean.Collector.WebApp.Config
{
    public static class ServicesConfig
    {
        public static void SetupServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddTransient<IEmployeeService, EmployeeService>();
        }
    }
}
