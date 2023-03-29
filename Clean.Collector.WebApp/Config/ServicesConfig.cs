
using Clean.Infrastructure.CleanDb.Services;

namespace Clean.Collector.WebApp.Config
{
    public static class ServicesConfig
    {
        public static void SetupServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddTransient<IRankService, RankService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ILocalizationService, LocalizationService>();
        }
    }
}
